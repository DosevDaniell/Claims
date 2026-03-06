using Claims.Application.Models;

namespace Claims.Application.Interfaces;
public interface IAuditQueue
{
    ValueTask EnqueueAsync(AuditEvent auditEvent, CancellationToken ct);
}
