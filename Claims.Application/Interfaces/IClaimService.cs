using Claims.Application.Models;

namespace Claims.Application.Interfaces;
public interface IClaimService
{
    Task<ClaimDto> CreateAsync(CreateClaimRequest request, CancellationToken ct);
    Task<IReadOnlyList<ClaimDto>> ListAsync(CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}

