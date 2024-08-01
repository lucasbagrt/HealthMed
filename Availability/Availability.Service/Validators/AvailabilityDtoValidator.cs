using Availability.Domain.Dtos;
using FluentValidation;

namespace Availability.Service.Validators
{
	public class ListAvailabilityDtoValidator : AbstractValidator<List<AvailabilityDto>>
	{
		public ListAvailabilityDtoValidator()
		{
			RuleForEach(x => x).SetValidator(new AvailabilityDtoValidator());
		}
	}

	public class AvailabilityDtoValidator : AbstractValidator<AvailabilityDto>
	{
		public AvailabilityDtoValidator()
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
