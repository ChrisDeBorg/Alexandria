using Alexandria.Domain.Entities;

namespace Alexandria.Domain.Relations;

public sealed class RelationType : AuditableEntity
{
    private RelationType() { }

    public RelationType(string name, string displayName)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Name is required.",
                nameof(name));

        Name = name.Trim();

        DisplayName = string.IsNullOrWhiteSpace(displayName)
            ? Name
            : displayName.Trim();
    }

    public string Name { get; private set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsSymmetric { get; set; }

    public bool IsTransitive { get; set; }

    public bool IsHierarchical { get; set; }

    public Guid? ParentRelationTypeId { get; private set; }

    public Guid? InverseRelationTypeId { get; set; }

    public void SetParentRelationType(Guid? parentRelationTypeId)
    {
        if (parentRelationTypeId == Id)
        {
            throw new ArgumentException(
                "A RelationType cannot be its own parent.",
                nameof(parentRelationTypeId));
        }

        ParentRelationTypeId = parentRelationTypeId;
    }
}