using Claims.Domain.Enums;

namespace Claims.Application.Models;
public sealed record CreateClaimRequest(
    ClaimType ClaimType,
    decimal DamageCost,
    string Description,
    DateOnly IncidentDate,
    CoverDto Cover);
