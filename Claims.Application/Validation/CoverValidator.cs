using Claims.Application.Exceptions;
using Claims.Application.Interfaces;
using Claims.Application.Models;

namespace Claims.Application.Validation;

public sealed class CoverValidator : ICoverValidator
{
    private readonly IClock _clock;
    private const int MaxInsurancePeriodYears = 1;

    public CoverValidator(IClock clock)
    {
        _clock = clock;
    }
    public void ValidateOrThrow(CoverDTO cover)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(cover.PolicyNumber))
            errors.Add("PolicyNumber is required.");

        if (cover.CoverageEnd < cover.CoverageStart)
            errors.Add("CoverageEnd must be after CoverageStart.");

        if (cover.MaxAmount <= 0)
            errors.Add("MaxAmount must be greater than zero.");

        if (string.IsNullOrWhiteSpace(cover.Currency))
            errors.Add("Currency is required.");

        var today = DateOnly.FromDateTime(_clock.UtcNow);
        if (cover.CoverageStart < today)
            errors.Add("CoverageStart cannot be in the past.");

        if (cover.CoverageEnd > cover.CoverageStart.AddYears(MaxInsurancePeriodYears))
            errors.Add($"Insurance period cannot exceed {MaxInsurancePeriodYears} year.");

        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}