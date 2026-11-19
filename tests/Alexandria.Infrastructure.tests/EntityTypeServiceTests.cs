using Alexandria.Application.EntityTypes;
using Alexandria.Domain.Ontology;
using Alexandria.Infrastructure.Persistence.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Alexandria.Infrastructure.Tests;

public class EntityTypeServiceTests
{
    [Fact]
    public async Task EntityType_Can_Be_Created_Read_Updated_And_Deleted()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<EntityTypeServiceTests>()
            .Build();

        var connectionString =
            configuration.GetConnectionString("Alexandria")
            ?? throw new InvalidOperationException(
                "Connection String 'Alexandria' fehlt.");

        var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        await using var context = new AlexandriaDbContext(options);

        // Teständerungen nach Abschluss zurückrollen.
        await using var transaction =
            await context.Database.BeginTransactionAsync();

        var entityTypeStore = new EntityTypeStore(context);
        var entityTypeService = new EntityTypeService(entityTypeStore);

        // CREATE
        var created = await entityTypeService.CreateAsync(
            new CreateEntityTypeRequest(
                "TestPerson",
                "Test Person",
                "EntityType für den Integrationstest"));

        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.Equal("TestPerson", created.Name);
        Assert.Equal("Test Person", created.DisplayName);
        Assert.Equal(
            "EntityType für den Integrationstest",
            created.Description);
        Assert.NotEqual(default, created.CreatedAt);
        Assert.Null(created.UpdatedAt);

        // READ: Einzelner EntityType
        var loaded = await entityTypeService.GetByIdAsync(created.Id);

        Assert.NotNull(loaded);
        Assert.Equal(created.Id, loaded.Id);
        Assert.Equal(created.Name, loaded.Name);

        // READ: Gesamtliste
        var allEntityTypes = await entityTypeService.GetAllAsync();

        Assert.Contains(allEntityTypes, x => x.Id == created.Id);

        // UPDATE
        var updated = await entityTypeService.UpdateAsync(
            created.Id,
            new UpdateEntityTypeRequest(
                "UpdatedTestPerson",
                "Updated Test Person",
                "Aktualisierte Beschreibung"));

        Assert.NotNull(updated);
        Assert.Equal("UpdatedTestPerson", updated.Name);
        Assert.Equal("Updated Test Person", updated.DisplayName);
        Assert.Equal(
            "Aktualisierte Beschreibung",
            updated.Description);

        // Audit-Felder
        Assert.Equal(created.CreatedAt, updated.CreatedAt);
        Assert.NotNull(updated.UpdatedAt);

        // DELETE
        var deleted = await entityTypeService.DeleteAsync(created.Id);

        Assert.True(deleted);

        // Nach dem Löschen darf der Datensatz nicht mehr existieren.
        var afterDelete =
            await entityTypeService.GetByIdAsync(created.Id);

        Assert.Null(afterDelete);

        // Zweites Löschen muss false zurückgeben.
        var deletedAgain =
            await entityTypeService.DeleteAsync(created.Id);

        Assert.False(deletedAgain);
    }
}