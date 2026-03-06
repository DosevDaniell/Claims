using Claims.Application.Exceptions;
using Claims.Application.Interfaces;
using Claims.Application.Models;

namespace Claims.Application.Validation;

public sealed class ClaimValidator : IClaimValidator
{
    private readonly IClock _clock;

    public ClaimValidator(IClock clock)
    {
        _clock = clock;
    }

    public void ValidateOrThrow(CreateClaimRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Description))
            errors.Add("Description is required.");

        var today = DateOnly.FromDateTime(_clock.UtcNow);

        if (request.IncidentDate > today)
            errors.Add("Incident date cannot be in the future.");

        if (request.IncidentDate < request.Cover.CoverageStart ||
            request.IncidentDate > request.Cover.CoverageEnd)
            errors.Add("Incident must be within cover period.");

        if (request.DamageCost > 100000)
            errors.Add("DamageCost cannot exceed 100000.");

        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}