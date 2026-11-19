namespace Alexandria.Application.EntityTypes;

public sealed record EntityTypeDto(
    Guid Id,
    string Name,
    string? DisplayName,
    string? Description,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);