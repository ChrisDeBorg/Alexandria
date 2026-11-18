namespace Alexandria.Application.Entities;

public sealed record UpdateEntityRequest(
    string Name,
    string? Description,
    Guid EntityTypeId);
