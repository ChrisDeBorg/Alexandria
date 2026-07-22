
using System.ComponentModel.DataAnnotations;

namespace Alexandria.Data.Entities;

public class Research
{
    [Key]
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Source> Sources { get; set; }
    public ICollection<BaseEntity> Entities { get; set; }
    public ICollection<Relation> Relations { get; set; }  = new HashSet<Relation>();
}
