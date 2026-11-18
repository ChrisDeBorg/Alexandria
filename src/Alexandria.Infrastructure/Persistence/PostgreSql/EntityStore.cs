using Alexandria.Application.Abstractions;
using Alexandria.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Infrastructure.Persistence.PostgreSql;

public sealed class EntityStore(
    AlexandriaDbContext dbContext) : IEntityStore
{
    public async Task<IReadOnlyList<BaseEntity>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Entities
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<BaseEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        // Absichtlich ohne AsNoTracking:
        // Die Entität soll bei Änderungen verfolgt werden.
        return await dbContext.Entities
            .SingleOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task AddAsync(
        BaseEntity entity,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await dbContext.Entities.AddAsync(
            entity,
            cancellationToken);
    }

    public void Remove(BaseEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        dbContext.Entities.Remove(entity);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}