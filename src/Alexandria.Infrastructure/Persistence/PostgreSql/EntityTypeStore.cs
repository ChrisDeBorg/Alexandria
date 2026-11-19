using Alexandria.Application.Abstractions;
using Alexandria.Domain.Ontology;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Infrastructure.Persistence.PostgreSql;

public sealed class EntityTypeStore(
    AlexandriaDbContext dbContext) : IEntityTypeStore
{
    public async Task<IReadOnlyList<EntityType>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.EntityTypes
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<EntityType?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Ohne AsNoTracking, damit Änderungen am EntityType
        // anschließend von EF Core erkannt werden.
        return await dbContext.EntityTypes
            .SingleOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task AddAsync(
        EntityType entityType,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entityType);

        await dbContext.EntityTypes.AddAsync(
            entityType,
            cancellationToken);
    }

    public void Remove(EntityType entityType)
    {
        ArgumentNullException.ThrowIfNull(entityType);

        dbContext.EntityTypes.Remove(entityType);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}