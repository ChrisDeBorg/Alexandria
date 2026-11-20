
namespace Alexandria.Application.Relations;

public sealed record RelationTypeDto(
    Guid Id,
    string Name,
    string DisplayName,
    string? Description,
    bool IsSymmetric,
    bool IsTransitive,
    bool IsHierarchical,
    Guid? ParentRelationTypeId);
