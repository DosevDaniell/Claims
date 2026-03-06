using Claims.Application.Interfaces;
using Claims.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Claims.API.Controllers;

[ApiController]
[Route("api/claims")]
public sealed class ClaimsController : ControllerBase
{
    private readonly IClaimService _service;
    public ClaimsController(IClaimService service) => _service = service;

    [HttpPost]
    [ProducesResponseType(typeof(ClaimDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateClaimRequest request, CancellationToken ct)
    {
        var created = await _service.CreateAsync(request, ct);
        return Created($"/api/claims/{created.Id}", created);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ClaimDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(CancellationToken ct)
        => Ok(await _service.ListAsync(ct));

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent();
    }
}