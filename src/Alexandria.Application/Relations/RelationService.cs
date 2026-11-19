
using Alexandria.Application.Abstractions;
using Alexandria.Domain.Relations;

namespace Alexandria.Application.Relations;

public sealed class RelationService(
    IRelationStore relationStore) : IRelationService
{
    public async Task<IReadOnlyList<RelationDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var relations = await relationStore.GetAllAsync(
            cancellationToken);

        return relations.Select(MapToDto).ToList();
    }

    public async Task<RelationDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var relation = await relationStore.GetByIdAsync(
            id,
            cancellationToken);

        return relation is null ? null : MapToDto(relation);
    }

    public async Task<RelationDto> CreateAsync(
        CreateRelationRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        ValidateRequest(
            request.SourceEntityId,
            request.TargetEntityId,
            request.RelationTypeId);

        var relation = new Relation(
            request.SourceEntityId,
            request.TargetEntityId,
            request.RelationTypeId)
        {
            ValidFrom = request.ValidFrom,
            ValidTo = request.ValidTo,
            Context = request.Context,
            IsInferred = request.IsInferred
        };

        await relationStore.AddAsync(relation, cancellationToken);
        await relationStore.SaveChangesAsync(cancellationToken);

        return MapToDto(relation);
    }

    public async Task<RelationDto?> UpdateAsync(
        Guid id,
        UpdateRelationRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        ValidateRequest(
            request.SourceEntityId,
            request.TargetEntityId,
            request.RelationTypeId);

        var relation = await relationStore.GetByIdAsync(
            id,
            cancellationToken);

        if (relation is null)
            return null;

        relation.ChangeDefinition(
            request.SourceEntityId,
            request.TargetEntityId,
            request.RelationTypeId);

        relation.ValidFrom = request.ValidFrom;
        relation.ValidTo = request.ValidTo;
        relation.Context = request.Context;
        relation.IsInferred = request.IsInferred;

        await relationStore.SaveChangesAsync(cancellationToken);

        return MapToDto(relation);
    }

    public async Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var relation = await relationStore.GetByIdAsync(
            id,
            cancellationToken);

        if (relation is null)
            return false;

        relationStore.Remove(relation);
        await relationStore.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static void ValidateRequest(
        Guid sourceEntityId,
        Guid targetEntityId,
        Guid relationTypeId)
    {
        if (sourceEntityId == Guid.Empty)
            throw new ArgumentException("SourceEntityId darf nicht leer sein.");

        if (targetEntityId == Guid.Empty)
            throw new ArgumentException("TargetEntityId darf nicht leer sein.");

        if (relationTypeId == Guid.Empty)
            throw new ArgumentException("RelationTypeId darf nicht leer sein.");

        if (sourceEntityId == targetEntityId)
            throw new ArgumentException(
                "Self-relations are not supported.");
    }

    private static RelationDto MapToDto(Relation relation)
    {
        return new RelationDto(
            relation.Id,
            relation.SourceEntityId,
            relation.TargetEntityId,
            relation.RelationTypeId,
            relation.ValidFrom,
            relation.ValidTo,
            relation.Context,
            relation.IsInferred,
            relation.CreatedAt,
            relation.UpdatedAt);
    }
}
