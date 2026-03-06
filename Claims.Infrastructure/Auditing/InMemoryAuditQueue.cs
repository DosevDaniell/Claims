using Claims.Application.Interfaces;
using Claims.Application.Models;
using System.Threading.Channels;

namespace Claims.Infrastructure.Auditing;
public sealed class InMemoryAuditQueue : IAuditQueue
{
    private readonly Channel<AuditEvent> _channel;

    public InMemoryAuditQueue(Channel<AuditEvent> channel)
    {
        _channel = channel;
    }

    public async ValueTask EnqueueAsync(AuditEvent auditEvent, CancellationToken ct)
    {
        await _channel.Writer.WriteAsync(auditEvent, ct);
    }
}