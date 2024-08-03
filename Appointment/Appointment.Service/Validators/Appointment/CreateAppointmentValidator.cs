using FluentValidation;
using Appointment.Domain.Dtos.Appointment;

namespace Appointment.Domain.Validators.Appointment;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentRequestDto>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.AvailabilityId)
            .GreaterThan(0).WithMessage("O ID da agenda deve ser um número positivo.");            
    }
}
