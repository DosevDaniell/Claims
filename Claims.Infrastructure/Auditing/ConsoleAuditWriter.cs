using Claims.Application.Interfaces;
using Claims.Application.Models;

namespace Claims.Infrastructure.Auditing;
public sealed class ConsoleAuditWriter : IAuditWriter
{
    public Task WriteAsync(AuditEvent auditEvent, CancellationToken ct)
    {
        Console.WriteLine(
            $"AUDIT | Action: {auditEvent.Action} | ClaimId: {auditEvent.ClaimId} | At: {auditEvent.OccurredAtUtc:O} | Description: {auditEvent.Description}");

        return Task.CompletedTask;
    }
}