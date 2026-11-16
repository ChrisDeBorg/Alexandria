using Alexandria.Domain.Relations;

namespace Alexandria.Domain.Entities;

public class Entity : BaseEntity
{
    public Entity() { }

    public Entity(string name, string entityType) : base(name, entityType)
    {
        Discriminator = "BaseEntity:Entity";
        Name = name;
    }

    public string? ShortName { get; set; } // "JFK", "IWF", "BlackRock"

    public bool IsVerified { get; set; } = false;

    public ICollection<Relation> Outgoing { get; set; } = [];
    public ICollection<Relation> Incoming { get; set; } = [];
}


