namespace Alexandria.Domain.Entities;

/// <summary>
/// Represents a person entity (alive, dead, undead, or fictional) in the Minerva persistence layer.
/// </summary>
public class Person : Entity
{
    public Person()
    {
    }
    public Person(string name) : base(name)
    {
    }
    public DateOnly? BirthDate { get; set; }
    public DateOnly? DeathDate { get; set; }          // Optional, for deceased persons

    public string? GivenName { get; set; }             // Vorname / Vornamen
    public string? FamilyName { get; set; }            // Nachname

    /// <summary>
    /// Calling name or preferred name, e.g. "Larry Fink" in "Laurence Fink".
    /// </summary>
    public string? CallingName { get; set; }

    /// <summary>
    /// Secondary name or middle name, e.g. "Douglas" in "Laurence Douglas Fink".
    /// </summary>
    public string? AdditionalName { get; set; }

    // Mehrere alternative Namen: Initialen, Rufname, Spitzname
    public List<string> AlternateNames { get; set; } = [];

    public GenderType? Gender { get; set; }

    public string? Ethnicity { get; set; }


    /*
     
additionalName	Zweitname / Mittelname (z. B. Douglas)
alternateName	Gängiger Kurzname, Spitzname, Künstlername (z. B. Larry Fink)
name	Vollständiger Display-Name (z. B. Laurence Douglas Fink)
description	Kurzbiografie / Kontext (z. B. "CEO von BlackRock")
sameAs	URL zu Wikipedia, Wikidata, o. Ä. für semantische Verknüpfung
     */
}


