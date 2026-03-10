using Claims.Domain.Enums;

namespace Claims.Application.Models;

public sealed record CoverDTO(
    string PolicyNumber,
    DateOnly CoverageStart,
    DateOnly CoverageEnd,
    decimal MaxAmount,
    string Currency,
    CoverType CoverType);
