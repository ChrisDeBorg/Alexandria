
using Alexandria.Domain.Entities;
using Alexandria.Domain.Relations;
using Alexandria.Infrastructure.Persistence.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Alexandria.Infrastructure.Tests;

public class PersistenceTests
{
    [Fact]
    public async Task Can_Save_And_Load_Entities_And_Relation()
    {
        // User Secrets des Testprojekts laden
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<PersistenceTests>()
            .Build();

        var connectionString =
            configuration.GetConnectionString("Alexandria")
            ?? throw new InvalidOperationException(
                "Connection String 'Alexandria' fehlt.");

        var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        await using var context = new AlexandriaDbContext(options);

        // Transaktion sorgt dafür, dass die Testdaten
        // nach erfolgreichem Test wieder zurückgerollt werden.
        await using var transaction =
            await context.Database.BeginTransactionAsync();

        var person = new Person("Max Mustermann")
        {
            EntityTypeId = Guid.NewGuid(),
            GivenName = "Max",
            FamilyName = "Mustermann",
            BirthDate = new DateOnly(1980, 5, 12),
            AlternateNames = ["M. Mustermann"],
            Gender = GenderType.Male
        };

        var organization = new Organization("Alexandria Test GmbH")
        {
            EntityTypeId = Guid.NewGuid(),
            FoundingDate = new DateOnly(2020, 1, 1),
            Website = "https://example.org"
        };

        var relationType = new RelationType(
            "worksFor",
            "arbeitet für")
        {
            Description = "Person arbeitet für Organisation."
        };

        context.Persons.Add(person);
        context.Organizations.Add(organization);
        context.RelationTypes.Add(relationType);

        await context.SaveChangesAsync();

        var relation = new Relation(
            person.Id,
            organization.Id,
            relationType.Id)
        {
            ValidFrom = new DateOnly(2022, 1, 1),
            Context = "Integrationstest",
            IsInferred = false
        };

        context.Relations.Add(relation);

        await context.SaveChangesAsync();

        // Change Tracker leeren, damit wir tatsächlich
        // erneut aus PostgreSQL materialisieren.
        context.ChangeTracker.Clear();

        var loadedPerson = await context.Persons
            .AsNoTracking()
            .SingleAsync(x => x.Id == person.Id);

        var loadedOrganization = await context.Organizations
            .AsNoTracking()
            .SingleAsync(x => x.Id == organization.Id);

        var loadedRelation = await context.Relations
            .AsNoTracking()
            .SingleAsync(x => x.Id == relation.Id);

        var loadedRelationType = await context.RelationTypes
            .AsNoTracking()
            .SingleAsync(x => x.Id == relationType.Id);

        // Assertions
        Assert.Equal("Max Mustermann", loadedPerson.Name);
        Assert.Equal("Max", loadedPerson.GivenName);
        Assert.Equal(new DateOnly(1980, 5, 12),
            loadedPerson.BirthDate);
        Assert.Contains("M. Mustermann",
            loadedPerson.AlternateNames);

        Assert.Equal("Alexandria Test GmbH",
            loadedOrganization.Name);

        Assert.Equal("worksFor", loadedRelationType.Name);

        Assert.Equal(person.Id, loadedRelation.SourceEntityId);
        Assert.Equal(organization.Id, loadedRelation.TargetEntityId);
        Assert.Equal(relationType.Id, loadedRelation.RelationTypeId);

        // TPH-Materialisierung prüfen
        var loadedEntity = await context.Entities
            .AsNoTracking()
            .SingleAsync(x => x.Id == person.Id);

        Assert.IsType<Person>(loadedEntity);

        var loadedBaseOrganization = await context.Entities
            .AsNoTracking()
            .SingleAsync(x => x.Id == organization.Id);

        Assert.IsType<Organization>(loadedBaseOrganization);
    }
}
