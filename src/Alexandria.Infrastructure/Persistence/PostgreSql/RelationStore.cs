
using Alexandria.Application.Abstractions;
using Alexandria.Domain.Relations;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Infrastructure.Persistence.PostgreSql;

public sealed class RelationStore(
    AlexandriaDbContext dbContext) : IRelationStore
{
    public async Task<IReadOnlyList<Relation>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Relations
            .AsNoTracking()
            .OrderBy(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Relation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Relations
            .SingleOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task AddAsync(
        Relation relation,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(relation);

        await dbContext.Relations.AddAsync(
            relation,
            cancellationToken);
    }

    public void Remove(Relation relation)
    {
        ArgumentNullException.ThrowIfNull(relation);

        dbContext.Relations.Remove(relation);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
