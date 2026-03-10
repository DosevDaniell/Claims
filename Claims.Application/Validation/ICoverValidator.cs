using Claims.Application.Models;

namespace Claims.Application.Validation;

public interface ICoverValidator
{
    /// <summary>
    /// Validates a cover request
    /// </summary>
    /// <param name="cover">The cover data do validate</param>
    /// <exception cref="ValidationException">
    /// Thrown when one or more validation ruler are violated
    /// </exception>
    void ValidateOrThrow(CoverDTO cover);
}