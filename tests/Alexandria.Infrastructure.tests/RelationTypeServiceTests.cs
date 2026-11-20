
using Alexandria.Application.Relations;
using Alexandria.Infrastructure.Persistence.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Alexandria.Infrastructure.Tests;

public class RelationTypeServiceTests
{
    [Fact]
    public async Task RelationTypes_Can_Be_Read_From_Database()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<RelationTypeServiceTests>()
            .Build();

        var connectionString =
            configuration.GetConnectionString("Alexandria")
            ?? throw new InvalidOperationException(
                "Connection String 'Alexandria' fehlt.");

        var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        await using var context = new AlexandriaDbContext(options);

        var store = new RelationTypeStore(context);
        var service = new RelationTypeService(store);

        // ACT
        var relationTypes = await service.GetAllAsync();

        // ASSERT: Datensätze vorhanden
        Assert.NotEmpty(relationTypes);

        // ASSERT: Sortierung
        var sortedNames = relationTypes
            .Select(x => x.Name)
            .OrderBy(x => x)
            .ToList();

        Assert.Equal(sortedNames, relationTypes.Select(x => x.Name));

        // ASSERT: Unsere Test-RelationTypes vorhanden
        Assert.Contains(relationTypes, x => x.Name == "worksFor");
        Assert.Contains(relationTypes, x => x.Name == "employs");
        Assert.Contains(relationTypes, x => x.Name == "owns");
        Assert.Contains(relationTypes, x => x.Name == "locatedIn");
        Assert.Contains(relationTypes, x => x.Name == "parentOf");

        // ASSERT: DTO-Mapping korrekt
        var knows = Assert.Single(
            relationTypes.Where(x => x.Name == "knows"));

        Assert.Equal("kennt", knows.DisplayName);
        Assert.True(knows.IsSymmetric);

        var locatedIn = Assert.Single(
            relationTypes.Where(x => x.Name == "locatedIn"));

        Assert.Equal("befindet sich in", locatedIn.DisplayName);
        Assert.True(locatedIn.IsTransitive);
        Assert.True(locatedIn.IsHierarchical);
    }
}
