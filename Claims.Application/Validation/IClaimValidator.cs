using Claims.Application.Models;

namespace Claims.Application.Validation;

public interface IClaimValidator
{
    void ValidateOrThrow(CreateClaimRequest request);
}
