using FluentValidation;
using User.Domain.Dtos.Auth;
using User.Domain.Enums;

namespace User.Service.Validators.Auth;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Informe o usuario");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Informe o primeiro nome");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Informe o email")
            .EmailAddress().WithMessage("Informe um email valido");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Informe o tipo de usuario");

        RuleFor(x => x.Crm)
          .NotEmpty().WithMessage("Informe o CRM")
          .When(x => x.Role == Role.DOCTOR);

        RuleFor(p => p.Password).ValidPassword();
    }
}