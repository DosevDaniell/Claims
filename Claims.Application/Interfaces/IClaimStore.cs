using Claims.Domain.Entities;

namespace Claims.Application.Interfaces;

public interface IClaimStore
{
    Task AddAsync(InsuranceClaim claim, CancellationToken ct);
    Task<IReadOnlyList<InsuranceClaim>> ListAsync(CancellationToken ct);
    Task<InsuranceClaim?> GetByIdAsync(Guid id, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}