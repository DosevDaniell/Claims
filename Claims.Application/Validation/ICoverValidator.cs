using Claims.Application.Models;

namespace Claims.Application.Validation;

public interface ICoverValidator
{
    void ValidateOrThrow(CoverDto cover);
}