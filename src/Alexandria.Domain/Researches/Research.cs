using Alexandria.Domain.Entities;

namespace Alexandria.Domain.Research;

public sealed class Research : AuditableEntity
{
    private Research() { }
    public Research(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title is required.", nameof(title));
        Title = title.Trim();
    }

    public string Title { get; private set; } = string.Empty;
    public string? Summary { get; set; }
    public string? Hypothesis { get; set; }
    public ICollection<ResearchEntity> Entities { get; private set; } = new List<ResearchEntity>();
}

public sealed class ResearchEntity
{
    public Guid ResearchId { get; set; }
    public Guid EntityId { get; set; }
    public string? Role { get; set; }
}
