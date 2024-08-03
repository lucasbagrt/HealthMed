using Appointment.Domain.Dtos.Appointment;
using Appointment.Domain.Dtos.Availability;
using FluentValidation;

namespace Appointment.Service.Validators.Availability;

public class CreateAvailabilityValidator : AbstractValidator<CreateAvailabilityDto>
{
    public CreateAvailabilityValidator()
    {       
        RuleFor(x => x.Date)
            .Must(BeAValidDate).WithMessage("A data da consulta deve ser uma data válida.");     
    }

    private bool BeAValidDate(DateTime date)
    {
        return date.Date >= DateTime.Now.Date;
    }
}
