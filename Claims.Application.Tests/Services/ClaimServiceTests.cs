using Claims.Application.Exceptions;
using Claims.Application.Models;
using Claims.Application.Services;
using Claims.Application.Tests.Fakes;
using Claims.Application.Validation;
using Claims.Domain.Enums;

namespace Claims.Application.Tests.Services;

public sealed class ClaimServiceTests
{
    private static (ClaimService service, TestClaimStore store, TestAuditQueue auditQueue, DateTime today)
        CreateSut(DateTime? utcNow = null)
    {
        var today = utcNow ?? new DateTime(2026, 3, 5, 0, 0, 0, DateTimeKind.Utc);

        var store = new TestClaimStore();
        var clock = new FakeClock { UtcNow = today };
        var auditQueue = new TestAuditQueue();

        var service = new ClaimService(
            store,
            new CoverValidator(clock),
            new ClaimValidator(clock),
            clock,
            auditQueue);

        return (service, store, auditQueue, today);
    }

    private static CoverDto ValidCover(DateTime today) =>
        new(
            PolicyNumber: "POL-123456",
            CoverageStart: DateOnly.FromDateTime(today),
            CoverageEnd: DateOnly.FromDateTime(today).AddMonths(6),
            MaxAmount: 5000,
            Currency: "EUR",
            CoverType: CoverType.Yacht);

    private static CreateClaimRequest ValidRequest(DateTime today) =>
        new(
            ClaimType: ClaimType.Collision,
            DamageCost: 13456,
            Description: "Accident happened in the marina",
            IncidentDate: DateOnly.FromDateTime(today),
            Cover: ValidCover(today));

    [Fact]
    public async Task CreateAsync_CreatesClaim_WhenValid()
    {
        var (service, _, _, today) = CreateSut();

        var created = await service.CreateAsync(
            ValidRequest(today),
            CancellationToken.None);

        Assert.NotEqual(Guid.Empty, created.Id);
    }

    [Fact]
    public async Task CreateAsync_ThrowsValidation_WhenDamageCostExceedsLimit()
    {
        var (service, _, _, today) = CreateSut();

        var req = ValidRequest(today) with { DamageCost = 100001m };

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.CreateAsync(req, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_ThrowsValidation_WhenIncidentDateInFuture()
    {
        var (service, _, _, today) = CreateSut();

        var req = ValidRequest(today) with
        {
            IncidentDate = DateOnly.FromDateTime(today).AddDays(5)
        };

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.CreateAsync(req, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_ThrowsValidation_WhenIncidentOutsideCoverPeriod()
    {
        var (service, _, _, today) = CreateSut();

        var req = ValidRequest(today) with
        {
            IncidentDate = DateOnly.FromDateTime(today).AddDays(-10)
        };

        await Assert.ThrowsAsync<ValidationException>(() =>
            service.CreateAsync(req, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteAsync_ThrowsNotFound_WhenMissing()
    {
        var (service, _, _, _) = CreateSut();

        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.DeleteAsync(Guid.NewGuid(), CancellationToken.None));
    }

    [Fact]
    public async Task DeleteAsync_RemovesClaim_WhenExists()
    {
        var (service, _, _, today) = CreateSut();

        var created = await service.CreateAsync(
            ValidRequest(today),
            CancellationToken.None);

        await service.DeleteAsync(created.Id, CancellationToken.None);

        var list = await service.ListAsync(CancellationToken.None);
        Assert.Empty(list);
    }

    [Fact]
    public async Task CreateAsync_EnqueuesAuditEvent_WhenValid()
    {
        var (service, _, auditQueue, today) = CreateSut();

        var created = await service.CreateAsync(
            ValidRequest(today),
            CancellationToken.None);

        Assert.Single(auditQueue.Events);
        Assert.Equal("CREATE", auditQueue.Events[0].Action);
        Assert.Equal(created.Id, auditQueue.Events[0].ClaimId);
    }

    [Fact]
    public async Task DeleteAsync_EnqueuesAuditEvent_WhenExists()
    {
        var (service, _, auditQueue, today) = CreateSut();

        var created = await service.CreateAsync(
            ValidRequest(today),
            CancellationToken.None);

        auditQueue.Events.Clear();

        await service.DeleteAsync(created.Id, CancellationToken.None);

        Assert.Single(auditQueue.Events);
        Assert.Equal("DELETE", auditQueue.Events[0].Action);
    }
}