using Alexandria.Domain.Entities;

namespace Alexandria.Domain.Entities;

public class BaseEntity : AuditableEntity
{
    public BaseEntity() { }

    public BaseEntity(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        Name = name.Trim();
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        Name = name.Trim();
        MarkUpdated();
    }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public Guid EntityTypeId { get; set; }

    public string? MetadataJson { get; set; }
}