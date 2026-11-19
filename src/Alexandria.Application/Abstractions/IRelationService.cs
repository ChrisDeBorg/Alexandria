
using Alexandria.Application.Relations;

namespace Alexandria.Application.Abstractions;

public interface IRelationService
{
    Task<IReadOnlyList<RelationDto>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<RelationDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<RelationDto> CreateAsync(
        CreateRelationRequest request,
        CancellationToken cancellationToken = default);

    Task<RelationDto?> UpdateAsync(
        Guid id,
        UpdateRelationRequest request,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
