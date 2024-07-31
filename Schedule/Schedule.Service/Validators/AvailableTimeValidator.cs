using FluentValidation;
using Schedule.Domain.Dtos;

namespace Schedule.Service.Validators
{
	public class AvailableTimeValidator : AbstractValidator<AvailableTimeDto>
	{
        public AvailableTimeValidator()
        {
			RuleFor(x => x.Start)
				.NotEmpty().WithMessage("Informe o início do horário disponível");

			RuleFor(x => x.End)
				.NotEmpty().WithMessage("Informe o fim do horário disponível");

			RuleFor(x => x.Start)
				.LessThan(x => x.End).WithMessage("Horário de início deve ser menor que o horário de fim");
		}
    }
}
