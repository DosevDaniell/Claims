using Claims.Domain.Enums;

namespace Claims.Application.Models;

public sealed record ClaimDto(
    Guid Id,
    ClaimType ClaimType,
    decimal DamageCost,
    string Description,
    DateOnly IncidentDate,
    string PolicyNumber,
    DateTime CreatedAtUtc);
