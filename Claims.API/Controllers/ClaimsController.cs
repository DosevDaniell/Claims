using Claims.Application.Interfaces;
using Claims.Application.Models;
using Claims.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Claims.API.Controllers;

/// <summary>
/// Provides endpoints for managing insurance claims
/// </summary>
[ApiController]
[Route("api/claims")]
public sealed class ClaimsController : ControllerBase
{
    private readonly IClaimService _service;
    public ClaimsController(IClaimService service) => _service = service;

    /// <summary>
    /// Creates a new claim
    /// </summary>
    /// <param name="request">The claim creation request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The created claim</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ClaimDTO), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateClaimRequest request, CancellationToken ct)
    {
        var claim = await _service.CreateAsync(request, ct);

        var dto = MapToDto(claim);

        return Created($"/api/claims/{dto.Id}", dto);
    }

    /// <summary>
    /// Returns all created claims
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>A list of existing claims</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<ClaimDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var claims = await _service.ListAsync(ct);

        var dtos = claims.Select(MapToDto).ToList();

        return Ok(dtos);
    }

    /// <summary>
    /// Deletes a claim by its id
    /// </summary>
    /// <param name="id">The id of the claim</param>
    /// <param name="ct">Cancellation token</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent();
    }
    private static ClaimDTO MapToDto(InsuranceClaim claim)
    {
        return new ClaimDTO(
            claim.Id,
            claim.ClaimType,
            claim.DamageCost,
            claim.Description,
            claim.IncidentDate,
            claim.Cover.PolicyNumber,
            claim.CreatedAtUtc);
    }
}