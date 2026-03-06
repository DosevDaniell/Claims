using Claims.Application.Models;

namespace Claims.Application.Interfaces;

public interface IAuditWriter
{
    Task WriteAsync(AuditEvent auditEvent, CancellationToken ct);
}
