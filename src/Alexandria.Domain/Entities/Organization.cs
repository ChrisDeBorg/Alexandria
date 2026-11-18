namespace Alexandria.Domain.Entities;

public class Organization : Entity
{
    protected Organization() { }
    public Organization(string name) : base(name) { }
    public DateOnly? FoundingDate { get; set; }
    public DateOnly? DissolutionDate { get; set; }
    public string? Website { get; set; }
}

public sealed class Company : Organization
{
    private Company() { }
    public Company(string name) : base(name) { }
    public string? StockSymbol { get; set; }
    public string? Exchange { get; set; }
    public int? EmployeeCount { get; set; }
    public decimal? AnnualRevenue { get; set; }
}
