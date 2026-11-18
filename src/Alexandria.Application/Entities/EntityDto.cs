namespace Alexandria.Application.Entities;

public sealed record EntityDto(
    Guid Id,
    string Name,
    string? Description,
    Guid EntityTypeId,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);