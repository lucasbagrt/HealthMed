using FluentValidation;
using Schedule.Domain.Dtos;

namespace Schedule.Service.Validators
{
	public class AddScheduleValidator : AbstractValidator<AddScheduleDto>
	{
		public AddScheduleValidator()
		{
			RuleFor(x => x.DoctorId)
				.NotEmpty().WithMessage("Informe o id do médico");

			RuleFor(x => x.AvailableTimes)
				.NotEmpty().WithMessage("Informe ao menos um horário disponível");

			RuleForEach(x => x.AvailableTimes)
				.SetValidator(new AvailableTimeValidator());
		}
	}
}
