using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Text.Json.Serialization;

namespace Alexandria.Domain.Entities;

public class BaseEntity : AuditableEntity
{
    public BaseEntity() { }
    protected BaseEntity(string name, string entityType)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        if (string.IsNullOrWhiteSpace(entityType)) throw new ArgumentException("Entity type is required.", nameof(entityType));
        

        Name = name.Trim();
        EntityType = entityType.StartsWith("BaseEntity:") ? entityType : $"BaseEntity:{entityType}";
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
        Name = name.Trim();
        MarkUpdated();
    }


    public required string Name { get; set; }

    public string EntityType { get; protected set; } = "BaseEntity";
    public string? Description { get; set; }

    public string Discriminator { get; set; } = "BaseEntity";

    public Guid EntityTypeId { get; set; }

    /// <summary>
    /// Flexible properties not yet represented
    /// by formal ontology definitions.
    /// </summary>
    public string? MetadataJson { get; set; }

}


