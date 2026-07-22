
using System.ComponentModel.DataAnnotations;

namespace Alexandria.Data.Entities;

public class Source
{
    [Key]
    public long Id { get; set; }
    public string Title { get; set; }          // z. B. "Russische Mafia – Wikipedia"
    public string? Url { get; set; }            // Link (falls vorhanden)
    public string SourceType { get; set; }     // z. B. "Website", "Book", "TV Documentary"

    public long? RelatedWorkId { get; set; }
    // Verknüpfung zu einem anderen Werk (optional)
    //public CreativeWork? RelatedWork { get; set; }
    public ICollection<Chunk> Chunks { get; set; } // für Vektorisierung

}
