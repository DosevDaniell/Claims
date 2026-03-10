using Claims.Application.Models;

namespace Claims.Application.Validation;

public interface IClaimValidator
{
    /// <summary>
    /// Validates a claim report
    /// </summary>
    /// <param name="request">Claim request to validate</param>
    /// <exception cref="ValidationException">
    /// Thrown when validatiot fails
    /// </exception>
    void ValidateOrThrow(CreateClaimRequest request);
}
