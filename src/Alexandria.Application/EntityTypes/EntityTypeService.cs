using Alexandria.Application.Abstractions;
using Alexandria.Domain.Ontology;

namespace Alexandria.Application.EntityTypes;

public sealed class EntityTypeService : IEntityTypeService
{
    private readonly IEntityTypeStore _entityTypeStore;

    public EntityTypeService(IEntityTypeStore entityTypeStore)
    {
        _entityTypeStore = entityTypeStore
            ?? throw new ArgumentNullException(nameof(entityTypeStore));
    }

    public async Task<IReadOnlyList<EntityTypeDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entityTypes = await _entityTypeStore.GetAllAsync(
            cancellationToken);

        return entityTypes
            .Select(MapToDto)
            .ToList();
    }

    public async Task<EntityTypeDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entityType = await _entityTypeStore.GetByIdAsync(
            id,
            cancellationToken);

        return entityType is null
            ? null
            : MapToDto(entityType);
    }

    public async Task<EntityTypeDto> CreateAsync(
        CreateEntityTypeRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entityType = new EntityType(
            request.Name,
            request.DisplayName,
            request.Description);

        await _entityTypeStore.AddAsync(
            entityType,
            cancellationToken);

        await _entityTypeStore.SaveChangesAsync(
            cancellationToken);

        return MapToDto(entityType);
    }

    public async Task<EntityTypeDto?> UpdateAsync(
        Guid id,
        UpdateEntityTypeRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entityType = await _entityTypeStore.GetByIdAsync(
            id,
            cancellationToken);

        if (entityType is null)
            return null;

        entityType.ChangeDefinition(
            request.Name,
            request.DisplayName,
            request.Description);

        await _entityTypeStore.SaveChangesAsync(
            cancellationToken);

        return MapToDto(entityType);
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entityType = await _entityTypeStore.GetByIdAsync(
            id,
            cancellationToken);

        if (entityType is null)
            return false;

        _entityTypeStore.Remove(entityType);

        await _entityTypeStore.SaveChangesAsync(
            cancellationToken);

        return true;
    }

    private static EntityTypeDto MapToDto(EntityType entityType)
    {
        return new EntityTypeDto(
            entityType.Id,
            entityType.Name,
            entityType.DisplayName,
            entityType.Description,
            entityType.CreatedAt,
            entityType.UpdatedAt);
    }
}