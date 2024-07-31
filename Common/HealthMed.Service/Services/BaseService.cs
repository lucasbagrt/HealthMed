using FluentValidation;
using FluentValidation.Results;
using HealthMed.Domain.Dtos.Default;
using HealthMed.Domain.Interfaces.Services;

namespace HealthMed.Service.Services;

public class BaseService : IBaseService
{
    public ValidationResult Validate<T>(T obj, AbstractValidator<T> validator)
    {
        if (obj == null)
            throw new Exception("Registros não detectados!");

        return validator.Validate(obj);
    }

    public DefaultServiceResponseDto DefaultResponse(string message, bool success)
    {
        return new DefaultServiceResponseDto
        {
            Message = message,
            Success = success
        };
    }
}
