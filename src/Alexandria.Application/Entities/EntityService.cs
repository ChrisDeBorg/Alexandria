using Alexandria.Application.Abstractions;
using Alexandria.Domain.Entities;

namespace Alexandria.Application.Entities;

public sealed class EntityService(
    IEntityStore entityStore) : IEntityService
{
    public async Task<IReadOnlyList<EntityDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await entityStore.GetAllAsync(cancellationToken);

        return entities
            .Select(MapToDto)
            .ToList();
    }

    public async Task<EntityDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await entityStore.GetByIdAsync(
            id,
            cancellationToken);

        return entity is null ? null : MapToDto(entity);
    }

    public async Task<EntityDto> CreateAsync(
        CreateEntityRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateEntityTypeId(request.EntityTypeId);

        var entity = new BaseEntity(request.Name)
        {
            Description = request.Description,
            EntityTypeId = request.EntityTypeId
        };

        await entityStore.AddAsync(entity, cancellationToken);
        await entityStore.SaveChangesAsync(cancellationToken);

        return MapToDto(entity);
    }

    public async Task<EntityDto?> UpdateAsync(
        Guid id,
        UpdateEntityRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ValidateEntityTypeId(request.EntityTypeId);

        var entity = await entityStore.GetByIdAsync(
            id,
            cancellationToken);

        if (entity is null)
            return null;

        entity.Rename(request.Name);
        entity.Description = request.Description;
        entity.EntityTypeId = request.EntityTypeId;

        await entityStore.SaveChangesAsync(cancellationToken);

        return MapToDto(entity);
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await entityStore.GetByIdAsync(
            id,
            cancellationToken);

        if (entity is null)
            return false;

        entityStore.Remove(entity);

        await entityStore.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static void ValidateEntityTypeId(Guid entityTypeId)
    {
        if (entityTypeId == Guid.Empty)
        {
            throw new ArgumentException(
                "EntityTypeId darf nicht leer sein.",
                nameof(entityTypeId));
        }
    }

    private static EntityDto MapToDto(BaseEntity entity)
    {
        return new EntityDto(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.EntityTypeId,
            entity.CreatedAt,
            entity.UpdatedAt);
    }
}