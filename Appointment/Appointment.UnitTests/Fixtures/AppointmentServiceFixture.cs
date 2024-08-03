using Appointment.Domain.Entities;
using Entities = Appointment.Domain.Entities;
using Appointment.Domain.Interfaces.Integration;
using Appointment.Domain.Interfaces.Repositories;
using Appointment.Service.Services;
using Appointment.UnitTests.Scenarios;
using AutoMapper;
using HealthMed.CrossCutting.Notifications;
using MassTransit;
using Moq;
using System.Linq.Expressions;
using Appointment.Domain.Dtos.Appointment;

namespace Appointment.UnitTests.Fixtures;

public class AppointmentServiceFixture
{
    public AppointmentService AppointmentService { get; }

    public AppointmentServiceFixture()
    {
        var mapperMock = new Mock<IMapper>();
        var appointmentRepositoryMock = new Mock<IAppointmentRepository>();
        var availabilityRepositoryMock = new Mock<IAvailabilityRepository>();
        var notificationContextMock = new Mock<NotificationContext>();
        var userIntegrationMock = new Mock<IUserIntegration>();
        var busMock = new Mock<IBus>();

        appointmentRepositoryMock.Setup(x => x.SelectAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => AppointmentServiceScenarios.Appointments.FirstOrDefault(a => a.Id == id));

        appointmentRepositoryMock.Setup(x => x.SelectAsync())
            .ReturnsAsync(AppointmentServiceScenarios.Appointments);

        appointmentRepositoryMock.Setup(x => x.SelectAsync(It.IsAny<Expression<Func<Entities.Appointment, bool>>>()))
            .ReturnsAsync((Expression<Func<Entities.Appointment, bool>> predicate) => AppointmentServiceScenarios.Appointments.AsQueryable().Where(predicate).ToList());

        userIntegrationMock.Setup(x => x.GetUserInfo(It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync((int userId, string token) => AppointmentServiceScenarios.Users.FirstOrDefault(u => u.Id == userId));

        availabilityRepositoryMock.Setup(x => x.FirstOrDefaultAsync(It.IsAny<Expression<Func<Availability, bool>>>()))
            .ReturnsAsync((Expression<Func<Availability, bool>> predicate) =>
            {
                var availabilityId = AppointmentServiceScenarios.Availabilities
                    .FirstOrDefault(predicate.Compile())?.Id;

                return availabilityId.HasValue
                    ? AppointmentServiceScenarios.Availabilities.FirstOrDefault(a => a.Id == availabilityId.Value)
                    : null;
            });

        mapperMock.Setup(x => x.Map<AppointmentDto>(It.IsAny<Entities.Appointment>()))
          .Returns((Entities.Appointment appointment) => new AppointmentDto
          {
              Id = appointment.Id,
              AvailabilityId = appointment.AvailabilityId,
              PatientId = appointment.PatientId,
              Status = appointment.Status,
          });

        mapperMock.Setup(x => x.Map<List<AppointmentDto>>(It.IsAny<List<Entities.Appointment>>()))
            .Returns((List<Entities.Appointment> appointments) => appointments.Select(appointment => new AppointmentDto
            {
                Id = appointment.Id,
                AvailabilityId = appointment.AvailabilityId,
                PatientId = appointment.PatientId,
                Status = appointment.Status,
            }).ToList());

        AppointmentService = new AppointmentService(
            mapperMock.Object,
            appointmentRepositoryMock.Object,
            availabilityRepositoryMock.Object,
            notificationContextMock.Object,
            userIntegrationMock.Object,
            busMock.Object);
    }
}