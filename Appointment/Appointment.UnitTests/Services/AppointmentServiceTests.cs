using Appointment.Domain.Dtos.Appointment;
using Appointment.Domain.Interfaces.Repositories;
using Appointment.Domain.Interfaces.Services;
using Appointment.UnitTests.Fixtures;
using Moq;

namespace Appointment.UnitTests.Services
{
    public class AppointmentServiceTests : IClassFixture<AppointmentServiceFixture>
    {
        private readonly IAppointmentService _appointmentService;
        private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
        private readonly Mock<IScheduleRepository> _mockScheduleRepository;

        public AppointmentServiceTests(AppointmentServiceFixture fixture)
        {
            _appointmentService = fixture.AppointmentService;
            _mockAppointmentRepository = new Mock<IAppointmentRepository>();
            _mockScheduleRepository = new Mock<IScheduleRepository>();
        }

        [Fact]
        public async Task GetAvailableSlotsAsync_ShouldReturnCorrectAvailableSlots()
        {
            // Arrange
            var doctorId = 1;
            var date = new DateTime(2024, 8, 1);

            var appointments = new List<Domain.Entities.Appointment>
            {
                new Domain.Entities.Appointment { DoctorId = doctorId, Date = date.Date, Time = new TimeSpan(10, 0, 0) },
                new Domain.Entities.Appointment { DoctorId = doctorId, Date = date.Date, Time = new TimeSpan(14, 0, 0) }
            };

            var workingHours = new List<WorkingHourDto>
            {
                new WorkingHourDto { StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(12, 0, 0) },
                new WorkingHourDto { StartTime = new TimeSpan(13, 0, 0), EndTime = new TimeSpan(17, 0, 0) }
            };

            var expectedAvailableSlots = new List<AvailableSlotDto>
            {
                new AvailableSlotDto { Time = new TimeSpan(9, 0, 0) },
                new AvailableSlotDto { Time = new TimeSpan(13, 0, 0) }
            };

            _mockAppointmentRepository
                .Setup(repo => repo.GetAppointmentsByDoctorIdAndDateAsync(It.IsAny<int>(), It.IsAny<DateTime>()))
                .ReturnsAsync(appointments);

            _mockScheduleRepository
                .Setup(repo => repo.GetWorkingHoursByDoctorIdAsync(It.IsAny<int>()))
                .ReturnsAsync(workingHours);

            // Act
            var result = await _appointmentService.GetAvailableSlotsAsync(doctorId, date);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAvailableSlots.Count, result.Count);
            foreach (var expectedSlot in expectedAvailableSlots)
            {
                Assert.Contains(result, slot => slot.Time == expectedSlot.Time);
            }
        }
    }
}
