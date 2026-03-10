using Claims.Application.Interfaces;
using Claims.Application.Models;
using System.Threading.Channels;

namespace Claims.API.BackgroundServices;

public sealed class AuditBackgroundService : BackgroundService
{
    private readonly Channel<AuditEvent> _channel;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<AuditBackgroundService> _logger;

    public AuditBackgroundService(
        Channel<AuditEvent> channel,
        IServiceProvider serviceProvider,
        ILogger<AuditBackgroundService> logger)
    {
        _channel = channel;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var auditEvent in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var auditWriter = scope.ServiceProvider.GetRequiredService<IAuditWriter>();

                await auditWriter.WriteAsync(auditEvent, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while processing audit event for claim {ClaimId}", auditEvent.ClaimId);
            }
        }
    }
}
