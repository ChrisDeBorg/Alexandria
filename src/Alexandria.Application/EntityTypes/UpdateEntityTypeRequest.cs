namespace Alexandria.Application.EntityTypes;

public sealed record UpdateEntityTypeRequest(
    string Name,
    string? DisplayName = null,
    string? Description = null);