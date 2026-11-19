using Alexandria.Domain.Entities;

namespace Alexandria.Domain.Relations;

public sealed class Relation : AuditableEntity
{
    private Relation() { }
    public Relation(Guid sourceEntityId, Guid targetEntityId, Guid relationTypeId)
    {
        if (sourceEntityId == targetEntityId) throw new ArgumentException("Self-relations are not supported by this constructor.");
        SourceEntityId = sourceEntityId;
        TargetEntityId = targetEntityId;
        RelationTypeId = relationTypeId;
    }
    public Guid SourceEntityId { get; private set; }
    public Guid TargetEntityId { get; private set; }
    public Guid RelationTypeId { get; private set; }
    public DateOnly? ValidFrom { get; set; }
    public DateOnly? ValidTo { get; set; }
    public string? Context { get; set; }
    public bool IsInferred { get; set; }

    public void ChangeDefinition(
    Guid sourceEntityId,
    Guid targetEntityId,
    Guid relationTypeId)
    {
        if (sourceEntityId == Guid.Empty)
            throw new ArgumentException("SourceEntityId darf nicht leer sein.");

        if (targetEntityId == Guid.Empty)
            throw new ArgumentException("TargetEntityId darf nicht leer sein.");

        if (relationTypeId == Guid.Empty)
            throw new ArgumentException("RelationTypeId darf nicht leer sein.");

        if (sourceEntityId == targetEntityId)
            throw new ArgumentException(
                "Self-relations are not supported.");

        SourceEntityId = sourceEntityId;
        TargetEntityId = targetEntityId;
        RelationTypeId = relationTypeId;

        MarkUpdated();
    }
}
