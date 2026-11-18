using Alexandria.Domain.Entities;

namespace Alexandria.Application.Abstractions;

public interface IEntityStore
{
    Task<IReadOnlyList<BaseEntity>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<BaseEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        BaseEntity entity,
        CancellationToken cancellationToken = default);

    void Remove(BaseEntity entity);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}