namespace Alexandria.Application.Relations;

public sealed record CreateRelationTypeRequest(
    string Name,
    string DisplayName,
    string? Description,
    Guid? ParentRelationTypeId,
    bool IsSymmetric,
    bool IsTransitive,
    bool IsHierarchical);