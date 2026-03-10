using Claims.Application.Interfaces;

namespace Claims.API.Infrastructure;

/// <summary>
/// Provides the current system time in UTC
/// </summary>
public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
