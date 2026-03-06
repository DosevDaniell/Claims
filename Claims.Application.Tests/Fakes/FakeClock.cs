using Claims.Application.Interfaces;

namespace Claims.Application.Tests.Fakes;

public sealed class FakeClock : IClock
{
    public DateTime UtcNow { get; set; } =
        new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}