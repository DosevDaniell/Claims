using Claims.Application.Models;
using Claims.Domain.Entities;

namespace Claims.Application.Interfaces;
public interface IClaimService
{
    /// <summary>
    /// Creates a new insurance claim
    /// </summary>
    /// <param name="request">Claim creation request</param>
    /// <param name="ct">Cancelation token</param>
    /// <returns>The created claim</returns>
    Task<InsuranceClaim> CreateAsync(CreateClaimRequest request, CancellationToken ct);

    /// <summary>
    /// Returns all stored claims
    /// </summary>
    /// <param name="ct">Cancelation token</param>
    /// <returns></returns>
    Task<IReadOnlyList<InsuranceClaim>> ListAsync(CancellationToken ct);

    /// <summary>
    /// Deletes a claim by id
    /// </summary>
    /// <param name="id">The id of the deleted claim</param>
    /// <param name="ct">Cancelation token</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken ct);
}

