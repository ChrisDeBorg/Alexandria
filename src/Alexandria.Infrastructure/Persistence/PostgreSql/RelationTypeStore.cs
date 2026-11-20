using Alexandria.Application.Abstractions;
using Alexandria.Domain.Relations;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.Infrastructure.Persistence.PostgreSql;

public sealed class RelationTypeStore(
    AlexandriaDbContext dbContext) : IRelationTypeStore
{
    public async Task<IReadOnlyList<RelationType>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await dbContext.RelationTypes
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<RelationType?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.RelationTypes
            .SingleOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task AddAsync(
        RelationType relationType,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(relationType);

        await dbContext.RelationTypes.AddAsync(
            relationType,
            cancellationToken);
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
