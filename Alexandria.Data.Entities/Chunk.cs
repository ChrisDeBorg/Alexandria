namespace Alexandria.Data.Entities
{
    public class Chunk
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public float[] Embedding { get; set; }
        public int PageNumber { get; set; }
    }
}