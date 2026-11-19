
using Alexandria.Application.Relations;
using Alexandria.Domain.Entities;
using Alexandria.Domain.Ontology;
using Alexandria.Domain.Relations;
using Alexandria.Infrastructure.Persistence.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Alexandria.Infrastructure.Tests;

public class RelationServiceTests
{
    [Fact]
    public async Task Relation_Can_Be_Created_Read_Updated_And_Deleted()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<RelationServiceTests>()
            .Build();

        var connectionString =
            configuration.GetConnectionString("Alexandria")
            ?? throw new InvalidOperationException(
                "Connection String 'Alexandria' fehlt.");

        var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        await using var context = new AlexandriaDbContext(options);

        // Testdaten werden nach Testende zurückgerollt.
        await using var transaction =
            await context.Database.BeginTransactionAsync();

        var entityType = new EntityType("TestRelationEntity");

        context.EntityTypes.Add(entityType);
        await context.SaveChangesAsync();

        var source = new BaseEntity("Test Source")
        {
            EntityTypeId = entityType.Id
        };

        var target = new BaseEntity("Test Target")
        {
            EntityTypeId = entityType.Id
        };

        var relationType = new RelationType(
            "testRelation",
            "Test Relation");

        context.Entities.AddRange(source, target);
        context.RelationTypes.Add(relationType);

        await context.SaveChangesAsync();

        var relationStore = new RelationStore(context);
        var relationService = new RelationService(relationStore);

        var validFrom = new DateOnly(2020, 1, 1);
        var validTo = new DateOnly(2025, 12, 31);

        // CREATE
        var created = await relationService.CreateAsync(
            new CreateRelationRequest(
                source.Id,
                target.Id,
                relationType.Id,
                validFrom,
                validTo,
                "Integrationstest",
                false));

        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal(source.Id, created.SourceEntityId);
        Assert.Equal(target.Id, created.TargetEntityId);
        Assert.Equal(relationType.Id, created.RelationTypeId);
        Assert.Equal(validFrom, created.ValidFrom);
        Assert.Equal(validTo, created.ValidTo);
        Assert.Equal("Integrationstest", created.Context);
        Assert.False(created.IsInferred);
        Assert.NotEqual(default, created.CreatedAt);
        Assert.Null(created.UpdatedAt);

        // READ: Einzelne Relation
        var loaded = await relationService.GetByIdAsync(created.Id);

        Assert.NotNull(loaded);
        Assert.Equal(created.Id, loaded.Id);
        Assert.Equal(source.Id, loaded.SourceEntityId);

        // READ: Gesamtliste
        var allRelations = await relationService.GetAllAsync();

        Assert.Contains(allRelations, x => x.Id == created.Id);

        // UPDATE
        var updated = await relationService.UpdateAsync(
            created.Id,
            new UpdateRelationRequest(
                source.Id,
                target.Id,
                relationType.Id,
                new DateOnly(2021, 1, 1),
                null,
                "Aktualisierter Kontext",
                true));

        Assert.NotNull(updated);
        Assert.Equal(new DateOnly(2021, 1, 1), updated.ValidFrom);
        Assert.Null(updated.ValidTo);
        Assert.Equal("Aktualisierter Kontext", updated.Context);
        Assert.True(updated.IsInferred);

        // Audit-Felder
        Assert.Equal(created.CreatedAt, updated.CreatedAt);
        Assert.NotNull(updated.UpdatedAt);

        // DELETE
        var deleted = await relationService.DeleteAsync(created.Id);

        Assert.True(deleted);

        var afterDelete =
            await relationService.GetByIdAsync(created.Id);

        Assert.Null(afterDelete);

        // Zweites Löschen
        var deletedAgain =
            await relationService.DeleteAsync(created.Id);

        Assert.False(deletedAgain);
    }
}
