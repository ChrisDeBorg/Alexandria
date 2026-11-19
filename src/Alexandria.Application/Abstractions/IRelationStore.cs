
using Alexandria.Domain.Relations;

namespace Alexandria.Application.Abstractions;

public interface IRelationStore
{
    Task<IReadOnlyList<Relation>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Relation?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Relation relation,
        CancellationToken cancellationToken = default);

    void Remove(Relation relation);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
