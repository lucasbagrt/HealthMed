using Moq;
using Appointment.Domain.Interfaces.Repositories;
using Appointment.Domain.Interfaces.Services;
using Appointment.Service.Services;
using Appointment.Domain.Dtos.Appointment;
using HealthMed.CrossCutting.Notifications;
using AutoMapper;
using Appointment.Domain.Interfaces.Integration;
using HealthMed.Domain.Dtos;
using MassTransit;

namespace Appointment.UnitTests.Fixtures
{
    public class AppointmentServiceFixture
    {
        public IAppointmentService AppointmentService { get; }

        public AppointmentServiceFixture()
        {
            var mockMapper = new Mock<IMapper>();

            var mockAppointmentRepository = new Mock<IAppointmentRepository>();
            mockAppointmentRepository.Setup(repo => repo.GetAppointmentsByDoctorIdAndDateAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync(new List<Domain.Entities.Appointment>
                {
                    new Domain.Entities.Appointment { Time = new TimeSpan(10, 0, 0) },
                    new Domain.Entities.Appointment { Time = new TimeSpan(14, 0, 0) }
                });

            var mockScheduleRepository = new Mock<IScheduleRepository>();
            mockScheduleRepository.Setup(repo => repo.GetWorkingHoursByDoctorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new List<WorkingHourDto>
                {
                    new WorkingHourDto { StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(12, 0, 0) },
                    new WorkingHourDto { StartTime = new TimeSpan(13, 0, 0), EndTime = new TimeSpan(17, 0, 0) }
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
                mockScheduleRepository.Object,
                mockNotificationContext.Object,
                mockIUserIntegration.Object,
                mockBus.Object
            );
        }
    }
}

