using Appointment.Domain.Dtos.Appointment;
using Appointment.Domain.Interfaces.Repositories;
using Appointment.Domain.Interfaces.Services;
using Appointment.UnitTests.Fixtures;
using Moq;
using AutoMapper;
using HealthMed.CrossCutting.Notifications;
using HealthMed.CrossCutting.QueueMessenge;
using MassTransit;
using Appointment.Domain.Interfaces.Integration;
using Availability.Domain.Interfaces.Repositories;

namespace Appointment.UnitTests.Services
{
    public class AppointmentServiceTests : IClassFixture<AppointmentServiceFixture>
    {
        private readonly IAppointmentService _appointmentService;
        private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
        private readonly Mock<IAvailabilityRepository> _mockAvailabilityRepository;
        private readonly Mock<IUserIntegration> _mockUserIntegration;
        private readonly Mock<IBus> _mockBus;
        private readonly Mock<IMapper> _mockMapper;

        public AppointmentServiceTests(AppointmentServiceFixture fixture)
        {
            _appointmentService = fixture.AppointmentService;
            _mockAppointmentRepository = new Mock<IAppointmentRepository>();
            _mockAvailabilityRepository = new Mock<IAvailabilityRepository>();
            _mockUserIntegration = new Mock<IUserIntegration>();
            _mockBus = new Mock<IBus>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task CreateAppointmentAsync_ShouldReturnSuccess_WhenAppointmentIsCreated()
        {
            // Arrange
            var requestDto = new CreateAppointmentRequestDto
            {
                DoctorId = 1,
                Date = new DateTime(2024, 8, 1),
                Time = new TimeSpan(9, 0, 0)
            };
            var patientId = 123;
            var token = "sample-token";

            _mockAppointmentRepository
                .Setup(repo => repo.ExistsAsync(requestDto.DoctorId, requestDto.Date, requestDto.Time))
                .ReturnsAsync(false);

            _mockMapper
                .Setup(m => m.Map<Domain.Entities.Appointment>(requestDto))
                .Returns(new Domain.Entities.Appointment());

            _mockBus
                .Setup(bus => bus.Publish(It.IsAny<CreateAppointment>(), default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _appointmentService.CreateAppointmentAsync(requestDto, patientId, token);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(StaticNotifications.AppointmentCreated.Message, result.Message);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_ShouldReturnSuccess_WhenAppointmentIsUpdated()
        {
            // Arrange
            var updateDto = new UpdateAppointmentRequestDto
            {
                AppointmentId = 1,
                Date = new DateTime(2024, 8, 2),
                Time = new TimeSpan(10, 0, 0)
            };
            var patientId = 123;

            var existingAppointment = new Domain.Entities.Appointment
            {
                Id = 1,
                PatientId = patientId,
                IsActive = true
            };

            _mockAppointmentRepository
                .Setup(repo => repo.SelectAsync(updateDto.AppointmentId))
                .ReturnsAsync(existingAppointment);

            _mockAppointmentRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Appointment>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _appointmentService.UpdateAppointmentAsync(updateDto, patientId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(StaticNotifications.AppointmentUpdated.Message, result.Message);
        }

        [Fact]
        public async Task CancelAppointmentAsync_ShouldReturnSuccess_WhenAppointmentIsCancelled()
        {
            // Arrange
            var appointmentId = 1;
            var patientId = 123;

            var existingAppointment = new Domain.Entities.Appointment
            {
                Id = appointmentId,
                PatientId = patientId,
                IsActive = true
            };

            _mockAppointmentRepository
                .Setup(repo => repo.SelectAsync(appointmentId))
                .ReturnsAsync(existingAppointment);

            _mockAppointmentRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<Domain.Entities.Appointment>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _appointmentService.CancelAppointmentAsync(appointmentId, patientId);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(StaticNotifications.AppointmentCancelled.Message, result.Message);
        }

        [Fact]
        public async Task GetAppointmentByIdAsync_ShouldReturnAppointmentDto()
        {
            // Arrange
            var appointmentId = 1;
            var appointment = new Domain.Entities.Appointment
            {
                Id = appointmentId,
                DoctorId = 1,
                PatientId = 123,
                Date = new DateTime(2024, 8, 1),
                Time = new TimeSpan(9, 0, 0)
            };

            var appointmentDto = new AppointmentDto
            {
                Id = appointmentId,
                PatientId = 123,
                Date = new DateTime(2024, 8, 1),
                Time = new TimeSpan(9, 0, 0)
            };

            _mockAppointmentRepository
                .Setup(repo => repo.SelectAsync(appointmentId))
                .ReturnsAsync(appointment);

            _mockMapper
                .Setup(m => m.Map<AppointmentDto>(appointment))
                .Returns(appointmentDto);

            // Act
            var result = await _appointmentService.GetAppointmentByIdAsync(appointmentId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(appointmentDto.Id, result.Id);
        }

        [Fact]
        public async Task GetAppointmentsByPatientIdAsync_ShouldReturnPatientAppointments()
        {
            // Arrange
            var patientId = 123;
            var appointments = new List<Domain.Entities.Appointment>
            {
                new Domain.Entities.Appointment { PatientId = patientId, IsActive = true },
                new Domain.Entities.Appointment { PatientId = patientId, IsActive = true },
                new Domain.Entities.Appointment { PatientId = 999, IsActive = true }
            };

            var appointmentDtos = new List<AppointmentDto>
            {
                new AppointmentDto(),
                new AppointmentDto()
            };

            _mockAppointmentRepository
                .Setup(repo => repo.SelectAsync())
                .ReturnsAsync(appointments);

            _mockMapper
                .Setup(m => m.Map<List<AppointmentDto>>(It.IsAny<List<Domain.Entities.Appointment>>()))
                .Returns(appointmentDtos);

            // Act
            var result = await _appointmentService.GetAppointmentsByPatientIdAsync(patientId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patientId, result.PatientId);
            Assert.Equal(2, result.Appointments.Count);
        }

        [Fact]
        public async Task GetAppointmentsByDoctorIdAsync_ShouldReturnDoctorAppointments()
        {
            // Arrange
            var doctorId = 1;
            var appointments = new List<Domain.Entities.Appointment>
            {
                new Domain.Entities.Appointment { DoctorId = doctorId, IsActive = true },
                new Domain.Entities.Appointment { DoctorId = doctorId, IsActive = true },
                new Domain.Entities.Appointment { DoctorId = 999, IsActive = true }
            };

            var appointmentDtos = new List<AppointmentDto>
            {
                new AppointmentDto(),
                new AppointmentDto()
            };

            _mockAppointmentRepository
                .Setup(repo => repo.SelectAsync())
                .ReturnsAsync(appointments);

            _mockMapper
                .Setup(m => m.Map<List<AppointmentDto>>(It.IsAny<List<Domain.Entities.Appointment>>()))
                .Returns(appointmentDtos);

            // Act
            var result = await _appointmentService.GetAppointmentsByDoctorIdAsync(doctorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(doctorId, result.DoctorId);
            Assert.Equal(2, result.Appointments.Count);
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

            var availabilities = new List<Availability.Domain.Entities.Availability>
            {
                new Availability.Domain.Entities.Availability { DoctorId = doctorId, Start = new DateTime(2024, 8, 1, 9, 0, 0), End = new DateTime(2024, 8, 1, 12, 0, 0) },
                new Availability.Domain.Entities.Availability { DoctorId = doctorId, Start = new DateTime(2024, 8, 1, 13, 0, 0), End = new DateTime(2024, 8, 1, 17, 0, 0) }
            };

            var expectedAvailableSlots = new List<AvailableSlotDto>
            {
                new AvailableSlotDto { Time = new TimeSpan(9, 0, 0) },
                new AvailableSlotDto { Time = new TimeSpan(9, 30, 0) },
                new AvailableSlotDto { Time = new TimeSpan(10, 0, 0) },
                new AvailableSlotDto { Time = new TimeSpan(10, 30, 0) },
                new AvailableSlotDto { Time = new TimeSpan(13, 0, 0) },
                new AvailableSlotDto { Time = new TimeSpan(13, 30, 0) },
                new AvailableSlotDto { Time = new TimeSpan(14, 0, 0) },
                new AvailableSlotDto { Time = new TimeSpan(14, 30, 0) }
            };

            _mockAppointmentRepository
                .Setup(repo => repo.GetAppointmentsByDoctorIdAndDateAsync(doctorId, date.Date))
                .ReturnsAsync(appointments);

            _mockAvailabilityRepository
                .Setup(repo => repo.SelectAsync())
                .ReturnsAsync(availabilities);

            // Act
            var result = await _appointmentService.GetAvailableSlotsAsync(doctorId, date);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAvailableSlots.Count, result.Count);
            Assert.Equal(expectedAvailableSlots.Select(slot => slot.Time).ToList(), result.Select(slot => slot.Time).ToList());
        }
    }
}
