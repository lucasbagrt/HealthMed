using Appointment.Domain.Enums;
using Appointment.Domain.Interfaces.Repositories;
using HealthMed.CrossCutting.QueueMessenge;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Appointment.Service.QueueMessege;

public class AppointmentConsumer : IConsumer<CreateAppointment>
{
    private readonly IServiceProvider _serviceProvider;

    public AppointmentConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Consume(ConsumeContext<CreateAppointment> context)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var appointmentRepository = scope.ServiceProvider.GetRequiredService<IAppointmentRepository>();

            var appointmentDB = new Domain.Entities.Appointment
            {
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Date = context.Message.Date,
                DoctorId = context.Message.DoctorId,
                PatientId = context.Message.PatientId,
                Time = context.Message.Time,
                Status = AppointmentStatus.Scheduled,
            };

            await appointmentRepository.InsertAsync(appointmentDB);
        }
    }
}
