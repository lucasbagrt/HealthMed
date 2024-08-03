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
            var _appointmentRepository = scope.ServiceProvider.GetRequiredService<IAppointmentRepository>();

            var appointmentDB = new Domain.Entities.Appointment
            {
                CreatedAt = DateTime.UtcNow,
                IsActive = true,                
                PatientId = context.Message.PatientId,
                AvailabilityId = context.Message.AvailabilityId,
                Status = AppointmentStatus.Scheduled,
            };

            await _appointmentRepository.InsertAsync(appointmentDB);
        }
    }
}
