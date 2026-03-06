using Claims.Domain.Enums;

namespace Claims.Application.Services.Premium;
public interface IPremiumCalculator
{
    decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType);
}
