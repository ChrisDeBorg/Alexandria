namespace Alexandria.Application.EntityTypes;

public sealed record CreateEntityTypeRequest(
    string Name,
    string? DisplayName = null,
    string? Description = null);
