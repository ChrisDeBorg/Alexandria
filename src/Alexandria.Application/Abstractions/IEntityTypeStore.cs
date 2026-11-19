using Alexandria.Domain.Ontology;

namespace Alexandria.Application.Abstractions;

public interface IEntityTypeStore
{
    Task<IReadOnlyList<EntityType>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<EntityType?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        EntityType entityType,
        CancellationToken cancellationToken = default);

    void Remove(EntityType entityType);

    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}