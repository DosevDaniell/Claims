using Claims.Application.Exceptions;
using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Application.Validation;
using Claims.Domain.Entities;

namespace Claims.Application.Services;

public sealed class ClaimService : IClaimService
{
    private readonly IClaimStore _store;
    private readonly ICoverValidator _coverValidator;
    private readonly IClaimValidator _claimValidator;
    private readonly IClock _clock;
    private readonly IAuditQueue _auditQueue;

    public ClaimService(
        IClaimStore store,
        ICoverValidator coverValidator,
        IClaimValidator claimValidator,
        IClock clock,
        IAuditQueue auditQueue)
    {
        _store = store;
        _coverValidator = coverValidator;
        _claimValidator = claimValidator;
        _clock = clock;
        _auditQueue = auditQueue;
    }

    public async Task<InsuranceClaim> CreateAsync(CreateClaimRequest request, CancellationToken ct)
    {
        _coverValidator.ValidateOrThrow(request.Cover);
        _claimValidator.ValidateOrThrow(request);

        var nowUtc = _clock.UtcNow;
        var createdDate = DateOnly.FromDateTime(nowUtc);

        if (createdDate < request.Cover.CoverageStart ||
            createdDate > request.Cover.CoverageEnd)
        {
            throw new ValidationException(new[]
            {
                "Created date must be within the cover period."
            });
        }

        var cover = new Cover(
            request.Cover.PolicyNumber,
            request.Cover.CoverageStart,
            request.Cover.CoverageEnd,
            request.Cover.MaxAmount,
            request.Cover.Currency,
            request.Cover.CoverType);

        var claim = InsuranceClaim.Create(
            Guid.NewGuid(),
            request.ClaimType,
            request.DamageCost,
            request.Description.Trim(),
            request.IncidentDate,
            cover,
            nowUtc);

        await _store.AddAsync(claim, ct);

        await _auditQueue.EnqueueAsync(
            new AuditEvent(
                Action: "CREATE",
                ClaimId: claim.Id,
                OccurredAtUtc: nowUtc,
                Description: $"Claim created for policy {claim.Cover.PolicyNumber}"),
            ct);

        return claim;
    }

    public async Task<IReadOnlyList<InsuranceClaim>> ListAsync(CancellationToken ct)
    {
        return await _store.ListAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var existing = await _store.GetByIdAsync(id, ct);

        if (existing is null)
            throw new NotFoundException($"Claim {id} not found.");

        await _store.DeleteAsync(id, ct);

        await _auditQueue.EnqueueAsync(
            new AuditEvent(
                Action: "DELETE",
                ClaimId: id,
                OccurredAtUtc: _clock.UtcNow,
                Description: "Claim deleted"),
            ct);
    }
}