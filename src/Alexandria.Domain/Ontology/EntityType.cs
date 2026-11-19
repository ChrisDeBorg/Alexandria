
namespace Alexandria.Domain.Ontology;

using Alexandria.Domain.Entities;

public sealed class EntityType : AuditableEntity
{
    private EntityType()
    {
    }

    public EntityType(
        string name,
        string? displayName = null,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Name is required.",
                nameof(name));

        Name = name.Trim();
        DisplayName = NormalizeOptional(displayName);
        Description = NormalizeOptional(description);
    }

    public string Name { get; private set; } = string.Empty;

    public string? DisplayName { get; private set; }

    public string? Description { get; private set; }

    public void ChangeDefinition(
        string name,
        string? displayName,
        string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Name is required.",
                nameof(name));

        Name = name.Trim();
        DisplayName = NormalizeOptional(displayName);
        Description = NormalizeOptional(description);

        MarkUpdated();
    }

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}
