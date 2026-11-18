
using Alexandria.Domain.Entities;
using Alexandria.Domain.Relations;
using Alexandria.Infrastructure.Persistence.PostgreSql;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Infrastructure.tests;


public class StructureTest
{

    [Fact]
    public void DbContext_ModelCanBeCreated()
    {
        var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
            .UseNpgsql(
                "Host=localhost;Database=alexandria;Username=kb_user;Password=GLMPTH03714TrKb")
            .Options;

        using var context = new AlexandriaDbContext(options);

        var entityTypes = context.Model.GetEntityTypes()
            .Select(x => x.ClrType.Name)
            .ToList();

        Assert.Contains(nameof(Person), entityTypes);
        Assert.Contains(nameof(Organization), entityTypes);
        Assert.Contains(nameof(Company), entityTypes);
        Assert.Contains(nameof(Relation), entityTypes);
        Assert.Contains(nameof(RelationType), entityTypes);
    }

    [Fact]
    public void ConnectionString_IsValid()
    {
        var connectionString = "Host=localhost;Database=Alexandria;Username=kb_user;Password=GLMPTH03714TrKb";
        var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
            .UseNpgsql(connectionString)
            .Options;
        using var context = new AlexandriaDbContext(options);
        // Try to open a connection to the database
        context.Database.OpenConnection();
        Assert.True(context.Database.CanConnect());
        context.Database.CloseConnection();

        //await using var scope = app.Services.CreateAsyncScope();

        //var dbContext = scope.ServiceProvider
        //    .GetRequiredService<AlexandriaDbContext>();

        //var canConnect = await dbContext.Database.CanConnectAsync();

        //if (!canConnect)
        //{
        //    throw new InvalidOperationException(
        //        "Die Verbindung zur PostgreSQL-Datenbank ist fehlgeschlagen.");
        //}
    }

}
