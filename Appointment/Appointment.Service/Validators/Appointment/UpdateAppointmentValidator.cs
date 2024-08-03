using FluentValidation;
using Appointment.Domain.Dtos.Appointment;

namespace Appointment.Domain.Validators.Appointment;

public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentRequestDto>
{
    public UpdateAppointmentValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("O ID do agendamento deve ser um número positivo.");

        RuleFor(x => x.AvailabilityId)
            .GreaterThan(0).WithMessage("O ID da agenda deve ser um número positivo.");            
    }        
}
