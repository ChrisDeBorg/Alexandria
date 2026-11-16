namespace Alexandria.Domain.Sources;

public class Chunk
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid SourceId { get; set; }

    public string Text { get; set; } = string.Empty;
    public float[] Embedding { get; set; } = [];
}