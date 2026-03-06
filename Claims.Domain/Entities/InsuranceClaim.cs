using Claims.Domain.Enums;

namespace Claims.Domain.Entities;
public sealed class InsuranceClaim
{
    public Guid Id { get; }
    public ClaimType ClaimType { get; }
    public decimal DamageCost { get; }
    public string Description { get; }
    public DateOnly IncidentDate { get; }
    public Cover Cover { get; }
    public DateTime CreatedAtUtc { get; }

    private InsuranceClaim(
        Guid id,
        ClaimType claimType,
        decimal damageCost,
        string description,
        DateOnly incidentDate,
        Cover cover,
        DateTime createdAtUtc)
    {
        Id = id;
        ClaimType = claimType;
        DamageCost = damageCost;
        Description = description;
        IncidentDate = incidentDate;
        Cover = cover;
        CreatedAtUtc = createdAtUtc;
    }

    public static InsuranceClaim Create(
        Guid id,
        ClaimType claimType,
        decimal damageCost,
        string description,
        DateOnly incidentDate,
        Cover cover,
        DateTime createdAtUtc)
        => new(id, claimType, damageCost, description, incidentDate, cover, createdAtUtc);
}