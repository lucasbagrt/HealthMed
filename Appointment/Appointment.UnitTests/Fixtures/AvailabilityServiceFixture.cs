using Appointment.Domain.Entities;
using Appointment.Domain.Interfaces.Repositories;
using Appointment.Service.Services;
using Appointment.UnitTests.Scenarios;
using AutoMapper;
using HealthMed.CrossCutting.Notifications;
using Moq;
using System.Linq.Expressions;

namespace Appointment.UnitTests.Fixtures;

public class AvailabilityServiceFixture
{
    public AvailabilityService AvailabilityService { get; }

    public AvailabilityServiceFixture()
    {
        var mapperMock = new Mock<IMapper>();
        var appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        var availabilityRepositoryMock = new Mock<IAvailabilityRepository>();
        var notificationContextMock = new Mock<NotificationContext>();

        // Configurar os mocks para os cenários de teste
        availabilityRepositoryMock.Setup(x => x.SelectAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => AvailabilityServiceScenarios.Availabilities.FirstOrDefault(a => a.Id == id));

        availabilityRepositoryMock.Setup(x => x.SelectAsync(It.IsAny<Expression<Func<Availability, bool>>>()))
            .ReturnsAsync((Expression<Func<Availability, bool>> predicate) => AvailabilityServiceScenarios.Availabilities.AsQueryable().Where(predicate).ToList());

        availabilityRepositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Availability, bool>>>()))
            .ReturnsAsync((Expression<Func<Availability, bool>> predicate) =>
            {
                var availabilityId = AppointmentServiceScenarios.Availabilities
                    .FirstOrDefault(predicate.Compile())?.Id;
            
                return availabilityId.HasValue
                    ? AppointmentServiceScenarios.Availabilities.FirstOrDefault(a => a.Id == availabilityId.Value)
                    : null;
            });

        appointmentRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Appointment.Domain.Entities.Appointment, bool>>>()))
            .ReturnsAsync((Expression<Func<Appointment.Domain.Entities.Appointment, bool>> predicate) => AvailabilityServiceScenarios.Appointments.AsQueryable().Any(predicate));

        mapperMock.Setup(x => x.Map<List<Appointment.Domain.Dtos.Availability.AvailabilityDto>>(It.IsAny<ICollection<Availability>>()))
            .Returns((ICollection<Availability> availabilities) => availabilities.Select(a => new Appointment.Domain.Dtos.Availability.AvailabilityDto
            {
                Id = a.Id,
                Date = a.Date,
                Time = a.Time,
                DoctorId = a.DoctorId
            }).ToList());

        AvailabilityService = new AvailabilityService(
            mapperMock.Object,
            appointmentRepositoryMock.Object,
            notificationContextMock.Object,
            availabilityRepositoryMock.Object);
    }
}
