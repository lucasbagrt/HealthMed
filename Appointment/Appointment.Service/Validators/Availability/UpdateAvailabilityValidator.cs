using Appointment.Domain.Dtos.Availability;
using FluentValidation;

namespace Appointment.Service.Validators.Availability;

public class UpdateAvailabilityValidator : AbstractValidator<UpdateAvailabilityDto>
{
    public UpdateAvailabilityValidator()
    {
        RuleFor(x => x.Id)
          .GreaterThan(0).WithMessage("O ID da agenda deve ser um número positivo.");

        RuleFor(x => x.Date)
            .Must(BeAValidDate).WithMessage("A data da consulta deve ser uma data válida.");        
    }

    private bool BeAValidDate(DateTime date)
    {
        return date.Date >= DateTime.Now.Date;
    }
}
