using Claims.Domain.Enums;

namespace Claims.Application.Services.Premium;
public interface IPremiumCalculator
{
    /// <summary>
    /// Calculates the insurance premium for a cover period
    /// </summary>
    /// <param name="startDate">The start date of the insurance period</param>
    /// <param name="endDate">
    /// The end date of the insurance period</param>
    /// <param name="coverType">The type of cover</param>
    /// <returns>The calculated premium amount</returns>
    decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType);
}
