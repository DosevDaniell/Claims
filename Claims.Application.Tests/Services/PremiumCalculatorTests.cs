using Claims.Application.Services.Premium;
using Claims.Domain.Enums;

namespace Claims.Application.Tests.Services;

public sealed class PremiumCalculatorTests
{
    [Fact]
    public void ComputePremium_Yacht_10Days_ReturnsExpectedPremium()
    {
        var calculator = new PremiumCalculator();

        var result = calculator.ComputePremium(
            new DateTime(2026, 1, 1),
            new DateTime(2026, 1, 11),
            CoverType.Yacht);

        Assert.Equal(13750m, result);
    }

    [Fact]
    public void ComputePremium_PassengerShip_10Days_ReturnsExpectedPremium()
    {
        var calculator = new PremiumCalculator();

        var result = calculator.ComputePremium(
            new DateTime(2026, 1, 1),
            new DateTime(2026, 1, 11),
            CoverType.PassengerShip);

        Assert.Equal(15000m, result);
    }

    [Fact]
    public void ComputePremium_Tanker_10Days_ReturnsExpectedPremium()
    {
        var calculator = new PremiumCalculator();

        var result = calculator.ComputePremium(
            new DateTime(2026, 1, 1),
            new DateTime(2026, 1, 11),
            CoverType.Tanker);

        Assert.Equal(18750m, result);
    }

    [Fact]
    public void ComputePremium_Yacht_40Days_AppliesFivePercentDiscountAfterDay30()
    {
        var calculator = new PremiumCalculator();

        var result = calculator.ComputePremium(
            new DateTime(2026, 1, 1),
            new DateTime(2026, 2, 10),
            CoverType.Yacht);

        var first30Days = 30 * 1375m;
        var next10Days = 10 * (1375m * 0.95m);
        var expected = first30Days + next10Days;

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ComputePremium_Cargo_200Days_AppliesProgressiveDiscounts()
    {
        var calculator = new PremiumCalculator();

        var result = calculator.ComputePremium(
            new DateTime(2026, 1, 1),
            new DateTime(2026, 7, 20),
            CoverType.ContainerShip);

        var dailyRate = 1250m * 1.3m;
        var first30 = 30 * dailyRate;
        var next150 = 150 * (dailyRate * 0.98m);
        var remaining20 = 20 * (dailyRate * 0.97m);
        var expected = first30 + next150 + remaining20;

        Assert.Equal(expected, result);
    }
}
