namespace Alexandria.Data.Entities
{
    public class Chunk
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public float[] Embedding { get; set; } = Array.Empty<float>();
        public int PageNumber { get; set; }
    }
}