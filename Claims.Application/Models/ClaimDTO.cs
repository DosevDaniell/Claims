using Claims.Domain.Enums;

namespace Claims.Application.Models;

public sealed record ClaimDTO(
    Guid Id,
    ClaimType ClaimType,
    decimal DamageCost,
    string Description,
    DateOnly IncidentDate,
    string PolicyNumber,
    DateTime CreatedAtUtc);
