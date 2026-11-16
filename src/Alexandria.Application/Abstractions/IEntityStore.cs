using Alexandria.Domain.Entities;

namespace Alexandria.Application.Abstractions;

public interface IEntityStore
{
    Task<BaseEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(BaseEntity entity, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
