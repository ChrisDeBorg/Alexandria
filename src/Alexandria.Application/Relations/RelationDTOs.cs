
namespace Alexandria.Application.Relations;

public sealed record CreateRelationRequest(
    Guid SourceEntityId,
    Guid TargetEntityId,
    Guid RelationTypeId,
    DateOnly? ValidFrom,
    DateOnly? ValidTo,
    string? Context,
    bool IsInferred);

public sealed record UpdateRelationRequest(
    Guid SourceEntityId,
    Guid TargetEntityId,
    Guid RelationTypeId,
    DateOnly? ValidFrom,
    DateOnly? ValidTo,
    string? Context,
    bool IsInferred);

public sealed record RelationDto(
    Guid Id,
    Guid SourceEntityId,
    Guid TargetEntityId,
    Guid RelationTypeId,
    DateOnly? ValidFrom,
    DateOnly? ValidTo,
    string? Context,
    bool IsInferred,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
