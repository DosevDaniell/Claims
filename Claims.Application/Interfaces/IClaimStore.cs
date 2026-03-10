using Claims.Domain.Entities;

namespace Claims.Application.Interfaces;

public interface IClaimStore
{
    /// <summary>
    /// Adds a new claim 
    /// </summary>
    /// <param name="claim">The claim to add</param>
    /// <param name="ct">Cancellation token</param>
    Task AddAsync(InsuranceClaim claim, CancellationToken ct);

    /// <summary>
    /// Returns all stored claims
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>A read-only list of claims</returns>
    Task<IReadOnlyList<InsuranceClaim>> ListAsync(CancellationToken ct);

    /// <summary>
    /// Retrieves a claim by its id
    /// </summary>
    /// <param name="id">The id of the claim</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The claim if found, otherwise null</returns>
    Task<InsuranceClaim?> GetByIdAsync(Guid id, CancellationToken ct);

    /// <summary>
    /// Deletes a claim by id
    /// </summary>
    /// <param name="id">The id of the claim</param>
    /// <param name="ct">Cancellation token</param>
    Task DeleteAsync(Guid id, CancellationToken ct);
}