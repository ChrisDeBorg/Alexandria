namespace Alexandria.Application.Relations;

public interface IRelationTypeService
{
    Task<IReadOnlyList<RelationTypeDto>> GetAllAsync(
        CancellationToken cancellationToken = default);
    Task<RelationTypeDto> CreateAsync(
        CreateRelationTypeRequest request,
        CancellationToken cancellationToken = default);

}
