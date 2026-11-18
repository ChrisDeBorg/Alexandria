using Alexandria.Application.Entities;

namespace Alexandria.Application.Abstractions;

public interface IEntityService
{
    Task<IReadOnlyList<EntityDto>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<EntityDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<EntityDto> CreateAsync(
        CreateEntityRequest request,
        CancellationToken cancellationToken = default);

    Task<EntityDto?> UpdateAsync(
        Guid id,
        UpdateEntityRequest request,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}