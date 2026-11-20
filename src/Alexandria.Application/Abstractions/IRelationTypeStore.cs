
using Alexandria.Domain.Relations;

namespace Alexandria.Application.Abstractions;

public interface IRelationTypeStore
{
    Task<IReadOnlyList<RelationType>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<RelationType?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        RelationType relationType,
        CancellationToken cancellationToken = default);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
