using Moq;
using Appointment.Domain.Interfaces.Repositories;
using Appointment.Domain.Interfaces.Services;
using Appointment.Service.Services;
using AutoMapper;
using Appointment.Domain.Interfaces.Integration;
using HealthMed.Domain.Dtos;
using MassTransit;
using Availability.Domain.Dtos;
using Appointment.Domain.Enums;
using Availability.Domain.Interfaces.Repositories;
using HealthMed.CrossCutting.Notifications;

namespace Appointment.UnitTests.Fixtures
{
    public class AppointmentServiceFixture
    {
        public IAppointmentService AppointmentService { get; }

        public AppointmentServiceFixture()
        {
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(m => m.Map<List<AvailabilityDto>>(It.IsAny<IEnumerable<Availability.Domain.Entities.Availability>>()))
                      .Returns((IEnumerable<Availability.Domain.Entities.Availability> source) =>
                          source.Select(a => new AvailabilityDto
                          {
                              Start = a.Start,
                              End = a.End
                          }).ToList());

            var mockAppointmentRepository = new Mock<IAppointmentRepository>();

            mockAppointmentRepository.Setup(repo => repo.ExistsAsync(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(false);

            mockAppointmentRepository.Setup(repo => repo.SelectAsync(It.IsAny<int>()))
                .ReturnsAsync(new Domain.Entities.Appointment
                {
                    Id = 1,
                    DoctorId = 1,
                    PatientId = 1,
                    Date = DateTime.Today,
                    Time = new TimeSpan(10, 0, 0),
                    Status = AppointmentStatus.Scheduled,
                    IsActive = true
                });

            var mockAvailabilityRepository = new Mock<IAvailabilityRepository>();

            mockAvailabilityRepository.Setup(repo => repo.SelectAsync())
                .ReturnsAsync(new List<Availability.Domain.Entities.Availability>
                {
                    new Availability.Domain.Entities.Availability { DoctorId = 1, Start = new DateTime(2024, 8, 1, 9, 0, 0), End = new DateTime(2024, 8, 1, 12, 0, 0) },
                    new Availability.Domain.Entities.Availability { DoctorId = 1, Start = new DateTime(2024, 8, 1, 13, 0, 0), End = new DateTime(2024, 8, 1, 17, 0, 0) }
                });

            var mockNotificationContext = new Mock<NotificationContext>();
            var mock = new Mock<NotificationContext>();

            var mockIUserIntegration = new Mock<IUserIntegration>();
            var userInfo = new UserInfoDto
            {
                Email = "test@example.com",
                FirstName = "John",
                Id = 1,
                LastName = "Doe",
                Name = "John Doe"
            };

            mockIUserIntegration.Setup(func => func.GetUserInfo(It.IsAny<int>(), It.IsAny<string>()))
                                .ReturnsAsync(userInfo);

            var mockBus = new Mock<IBus>();

            AppointmentService = new AppointmentService(
                mockMapper.Object,
                mockAppointmentRepository.Object,
                mockAvailabilityRepository.Object,
                mockIUserIntegration.Object,
                mockBus.Object
            );
        }
    }
}
