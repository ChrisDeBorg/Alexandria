namespace Alexandria.Domain.Entities;

public abstract class AuditableEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; protected set; }

    public void MarkUpdated(DateTimeOffset? at = null) => UpdatedAt = at ?? DateTimeOffset.UtcNow;
}
