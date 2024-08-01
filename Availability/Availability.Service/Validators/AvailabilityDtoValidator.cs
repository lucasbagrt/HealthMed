using FluentValidation;
using Availability.Domain.Dtos;

namespace Availability.Service.Validators
{
	public class AvailabilityDtoValidator : AbstractValidator<AvailabilityDto>
	{
		public AvailabilityDtoValidator()
		{
			RuleFor(x => x.AvailableTimes)
				.NotEmpty().WithMessage("Informe ao menos um horário disponível");

			RuleForEach(x => x.AvailableTimes)
				.SetValidator(new AvailableTimeValidator());
		}
	}
}
