namespace Alexandria.Application.EntityTypes;

public interface IEntityTypeService
{
    Task<IReadOnlyList<EntityTypeDto>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<EntityTypeDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<EntityTypeDto> CreateAsync(
        CreateEntityTypeRequest request,
        CancellationToken cancellationToken = default);

    Task<EntityTypeDto?> UpdateAsync(
        Guid id,
        UpdateEntityTypeRequest request,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}