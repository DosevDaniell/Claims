using Claims.Domain.Enums;

namespace Claims.Domain.Entities;

public sealed record Cover(
    string PolicyNumber,
    DateOnly CoverageStart,
    DateOnly CoverageEnd,
    decimal MaxAmount,
    string Currency,
    CoverType CoverType);