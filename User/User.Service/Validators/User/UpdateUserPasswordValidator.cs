using FluentValidation;
using User.Domain.Dtos.User;

namespace User.Service.Validators.User;

public class UpdateUserPasswordValidator : AbstractValidator<UpdateUserPasswordDto>
{
    public UpdateUserPasswordValidator()
    {
        RuleFor(p => p.NewPassword).ValidPassword();
    }
}
