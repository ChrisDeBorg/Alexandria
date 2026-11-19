using Alexandria.Domain.Ontology;
using Alexandria.Application.Entities;
using Alexandria.Infrastructure.Persistence.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Alexandria.Infrastructure.Tests;

public class EntityServiceTests
{
    [Fact]
    public async Task Entity_Can_Be_Created_Read_Updated_And_Deleted()
    {
        // Arrange: PostgreSQL-Verbindung aus User Secrets laden
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<EntityServiceTests>()
            .Build();

        var connectionString =
            configuration.GetConnectionString("Alexandria")
            ?? throw new InvalidOperationException(
                "Connection String 'Alexandria' fehlt.");

        var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        await using var context = new AlexandriaDbContext(options);

        // Sämtliche Teständerungen werden am Ende zurückgerollt.
        await using var transaction =
            await context.Database.BeginTransactionAsync();

        var entityStore = new EntityStore(context);
        var entityService = new EntityService(entityStore);

        var entityType = new EntityType(
                "TestEntity",
                "Test Entity");

        context.EntityTypes.Add(entityType);
        await context.SaveChangesAsync();

        var entityTypeId = entityType.Id;

        // CREATE
        var created = await entityService.CreateAsync(
            new CreateEntityRequest(
                "  Alexandria Test Entity  ",
                "Beschreibung für den Integrationstest",
                entityTypeId));

        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal("Alexandria Test Entity", created.Name);
        Assert.Equal(
            "Beschreibung für den Integrationstest",
            created.Description);
        Assert.Equal(entityTypeId, created.EntityTypeId);
        Assert.NotEqual(default, created.CreatedAt);
        Assert.Null(created.UpdatedAt);

        // READ: Einzelne Entität
        var loaded = await entityService.GetByIdAsync(created.Id);

        Assert.NotNull(loaded);
        Assert.Equal(created.Id, loaded.Id);
        Assert.Equal(created.Name, loaded.Name);

        // READ: Gesamtliste
        var allEntities = await entityService.GetAllAsync();

        Assert.Contains(allEntities, x => x.Id == created.Id);

        // UPDATE
        var updated = await entityService.UpdateAsync(
            created.Id,
            new UpdateEntityRequest(
                "Alexandria Updated Entity",
                "Aktualisierte Beschreibung",
                entityTypeId));

        Assert.NotNull(updated);
        Assert.Equal("Alexandria Updated Entity", updated.Name);
        Assert.Equal(
            "Aktualisierte Beschreibung",
            updated.Description);

        // Audit-Felder prüfen
        Assert.Equal(created.CreatedAt, updated.CreatedAt);
        Assert.NotNull(updated.UpdatedAt);

        // DELETE
        var deleted = await entityService.DeleteAsync(created.Id);

        Assert.True(deleted);

        // Nach dem Löschen darf die Entität nicht mehr gefunden werden.
        var afterDelete =
            await entityService.GetByIdAsync(created.Id);

        Assert.Null(afterDelete);

        // Zweites Löschen muss false zurückgeben.
        var deletedAgain =
            await entityService.DeleteAsync(created.Id);

        Assert.False(deletedAgain);
    }
}
