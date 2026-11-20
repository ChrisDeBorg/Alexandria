using Alexandria.Application.Abstractions;
using Alexandria.Domain.Relations;

namespace Alexandria.Application.Relations;

public sealed class RelationTypeService(
    IRelationTypeStore relationTypeStore) : IRelationTypeService
{
    public async Task<IReadOnlyList<RelationTypeDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var relationTypes = await relationTypeStore.GetAllAsync(
            cancellationToken);

        return relationTypes
            .Select(MapToDto)
            .ToList();
    }

    public async Task<RelationTypeDto> CreateAsync(
        CreateRelationTypeRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException(
                "Name is required.",
                nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.DisplayName))
        {
            throw new ArgumentException(
                "DisplayName is required.",
                nameof(request));
        }

        if (request.ParentRelationTypeId.HasValue)
        {
            var parentRelationType =
                await relationTypeStore.GetByIdAsync(
                    request.ParentRelationTypeId.Value,
                    cancellationToken);

            if (parentRelationType is null)
            {
                throw new InvalidOperationException(
                    "The specified parent RelationType does not exist.");
            }
        }

        var relationType = new RelationType(
            request.Name,
            request.DisplayName);

        relationType.Description = request.Description;
        relationType.IsSymmetric = request.IsSymmetric;
        relationType.IsTransitive = request.IsTransitive;
        relationType.IsHierarchical = request.IsHierarchical;

        relationType.SetParentRelationType(
            request.ParentRelationTypeId);

        await relationTypeStore.AddAsync(
            relationType,
            cancellationToken);

        await relationTypeStore.SaveChangesAsync(
            cancellationToken);

        return MapToDto(relationType);
    }

    private static RelationTypeDto MapToDto(
        RelationType relationType)
    {
        return new RelationTypeDto(
            relationType.Id,
            relationType.Name,
            relationType.DisplayName,
            relationType.Description,
            relationType.IsSymmetric,
            relationType.IsTransitive,
            relationType.IsHierarchical,
            relationType.ParentRelationTypeId);
    }
}