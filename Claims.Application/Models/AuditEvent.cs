
namespace Claims.Application.Models;
public sealed record AuditEvent(
    string Action,
    Guid ClaimId,
    DateTime OccurredAtUtc,
    string? Description = null);
