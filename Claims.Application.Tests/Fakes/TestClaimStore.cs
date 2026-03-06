using Claims.Application.Interfaces;
using Claims.Domain.Entities;

namespace Claims.Application.Tests.Fakes;

public sealed class TestClaimStore : IClaimStore
{
    private readonly Dictionary<Guid, InsuranceClaim> _db = new();

    public Task AddAsync(InsuranceClaim claim, CancellationToken ct)
    {
        _db[claim.Id] = claim;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<InsuranceClaim>> ListAsync(CancellationToken ct)
        => Task.FromResult((IReadOnlyList<InsuranceClaim>)_db.Values.ToList());

    public Task<InsuranceClaim?> GetByIdAsync(Guid id, CancellationToken ct)
        => Task.FromResult(_db.TryGetValue(id, out var claim) ? claim : null);

    public Task DeleteAsync(Guid id, CancellationToken ct)
    {
        _db.Remove(id);
        return Task.CompletedTask;
    }
}