using FluentValidation;
using Appointment.Domain.Dtos.Appointment;

public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentRequestDto>
{
    public UpdateAppointmentValidator()
    {
        RuleFor(x => x.PatientId)
            .GreaterThan(0).WithMessage("O ID do paciente deve ser um número positivo.");

        RuleFor(x => x.Date)
            .GreaterThan(DateTime.Now.Date).WithMessage("A data da consulta deve ser uma data futura.");

        RuleFor(x => x.Time)
            .InclusiveBetween(TimeSpan.FromHours(8), TimeSpan.FromHours(18))
            .WithMessage("O horário da consulta deve estar entre 08:00 e 18:00.");

        RuleFor(x => x.Date)
            .Must(BeAValidDate).WithMessage("A data da consulta deve ser uma data válida.");

        RuleFor(x => x.Time)
            .Must(BeAValidTime).WithMessage("O horário da consulta deve ser um horário válido.");
    }

    private bool BeAValidDate(DateTime date)
    {
        return date.Date > DateTime.Now.Date;
    }

    private bool BeAValidTime(TimeSpan time)
    {
        return time >= TimeSpan.FromHours(8) && time <= TimeSpan.FromHours(18);
    }
}
