using Claims.Domain.Enums;

namespace Claims.Application.Services.Premium;

public sealed class PremiumCalculator : IPremiumCalculator
{
    private const decimal BaseDayRate = 1250m;

    public decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        var multiplier = GetMultiplier(coverType);

        var dailyRate = BaseDayRate * multiplier;
        var totalDays = (endDate - startDate).Days;

        decimal total = 0;

        for (int day = 0; day < totalDays; day++)
        {
            var rate = ApplyDiscount(day, dailyRate, coverType);
            total += rate;
        }

        return total;
    }

    private static decimal GetMultiplier(CoverType coverType)
    {
        return coverType switch
        {
            CoverType.Yacht => 1.1m,
            CoverType.PassengerShip => 1.2m,
            CoverType.Tanker => 1.5m,
            _ => 1.3m
        };
    }

    private static decimal ApplyDiscount(int day, decimal dailyRate, CoverType coverType)
    {
        bool isYacht = coverType == CoverType.Yacht;

        if (day < 30)
            return dailyRate;

        if (day < 180)
            return dailyRate * (isYacht ? 0.95m : 0.98m);

        return dailyRate * (isYacht ? 0.92m : 0.97m);

    }
}