using FluentValidation;
using FluentValidation.Results;
using HealthMed.Domain.Dtos.Default;

namespace HealthMed.Domain.Interfaces.Services;

public interface IBaseService
{
    ValidationResult Validate<T>(T obj, AbstractValidator<T> validator);
    DefaultServiceResponseDto DefaultResponse(string message, bool success);
}
