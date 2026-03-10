using Claims.Application.Services.Premium;
using Claims.Domain.Enums;

public sealed class PremiumCalculator : IPremiumCalculator
{
    private const decimal BaseDayRate = 1250m;
    private const int FirstBandDays = 30;
    private const int SecondBandDays = 180;

    private sealed record RateConfiguration(
        decimal Multiplier,
        decimal SecondBandDiscount,
        decimal ThirdBandDiscount);

    private static readonly Dictionary<CoverType, RateConfiguration> RateConfigurations =
        new()
        {
            {
                CoverType.Yacht,
                new RateConfiguration(1.10m, 0.95m, 0.92m)
            },
            {
                CoverType.PassengerShip,
                new RateConfiguration(1.20m, 0.98m, 0.97m)
            },
            {
                CoverType.Tanker,
                new RateConfiguration(1.50m, 0.98m, 0.97m)
            }
        };

    public decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        var config = RateConfigurations.TryGetValue(coverType, out var value)
            ? value
            : new RateConfiguration(1.30m, 0.98m, 0.97m);

        var dailyRate = BaseDayRate * config.Multiplier;
        var totalDays = (endDate - startDate).Days;

        decimal total = 0;

        for (int day = 0; day < totalDays; day++)
        {
            total += ApplyDiscount(day, dailyRate, config);
        }

        return total;
    }

    private static decimal ApplyDiscount(int day, decimal dailyRate, RateConfiguration config)
    {
        if (day < FirstBandDays)
            return dailyRate;

        if (day < SecondBandDays)
            return dailyRate * config.SecondBandDiscount;

        return dailyRate * config.ThirdBandDiscount;
    }
}