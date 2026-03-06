using Claims.Application.Interfaces;
using Claims.Application.Models;

namespace Claims.Application.Tests.Fakes;

public sealed class TestAuditQueue : IAuditQueue
{
    public List<AuditEvent> Events { get; } = new();

    public ValueTask EnqueueAsync(AuditEvent auditEvent, CancellationToken ct)
    {
        Events.Add(auditEvent);
        return ValueTask.CompletedTask;
    }
}