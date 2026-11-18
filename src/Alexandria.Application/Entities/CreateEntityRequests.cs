namespace Alexandria.Application.Entities;

public sealed record CreateEntityRequest(
    string Name,
    string? Description,
    Guid EntityTypeId);
