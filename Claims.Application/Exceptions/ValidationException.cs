
namespace Claims.Application.Exceptions;

public sealed class ValidationException : Exception
{
    public IReadOnlyList<string> Errors { get; }

    public ValidationException(IReadOnlyList<string> errors)
        : base("Validation failed")
    {
        Errors = errors;
    }
}
