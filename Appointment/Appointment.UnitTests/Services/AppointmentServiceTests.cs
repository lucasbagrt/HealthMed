using Appointment.UnitTests.Fixtures;
using Appointment.UnitTests.Scenarios;
using HealthMed.CrossCutting.Notifications;

namespace Appointment.UnitTests.Services;

public class AppointmentServiceTests : IClassFixture<AppointmentServiceFixture>
{
    private readonly AppointmentServiceFixture _fixture;

    public AppointmentServiceTests(AppointmentServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateAsync_ValidAppointment_ReturnsSuccess()
    {
        // Arrange
        var createAppointmentRequestDto = AppointmentServiceScenarios.ValidCreateAppointmentRequestDto();
        int patientId = 1;
        string token = "valid_token";

        // Act
        var result = await _fixture.AppointmentService.CreateAsync(createAppointmentRequestDto, patientId, token);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(StaticNotifications.AppointmentCreated.Message, result.Message);
    }

    [Fact]
    public async Task CreateAsync_InvalidAvailability_ReturnsDefault()
    {
        // Arrange
        var createAppointmentRequestDto = AppointmentServiceScenarios.InvalidCreateAppointmentRequestDto();
        int patientId = 1;
        string token = "valid_token";

        // Act
        var result = await _fixture.AppointmentService.CreateAsync(createAppointmentRequestDto, patientId, token);

        // Assert
        Assert.Equal(default, result);
    }

    [Fact]
    public async Task UpdateAsync_ValidAppointment_ReturnsSuccess()
    {
        // Arrange
        var updateAppointmentRequestDto = AppointmentServiceScenarios.ValidUpdateAppointmentRequestDto();
        int patientId = 1;

        // Act
        var result = await _fixture.AppointmentService.UpdateAsync(updateAppointmentRequestDto, patientId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(StaticNotifications.AppointmentUpdated.Message, result.Message);
    }

    [Fact]
    public async Task UpdateAsync_InvalidAvailability_ReturnsDefault()
    {
        // Arrange
        var updateAppointmentRequestDto = AppointmentServiceScenarios.InvalidUpdateAppointmentRequestDto();
        int patientId = 1;

        // Act
        var result = await _fixture.AppointmentService.UpdateAsync(updateAppointmentRequestDto, patientId);

        // Assert
        Assert.Equal(default, result);
    }

    [Fact]
    public async Task CancelAsync_ValidAppointment_ReturnsSuccess()
    {
        // Arrange
        int appointmentId = 1;
        int patientId = 1;

        // Act
        var result = await _fixture.AppointmentService.CancelAsync(appointmentId, patientId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(StaticNotifications.AppointmentCancelled.Message, result.Message);
    }

    [Fact]
    public async Task CancelAsync_InvalidPatient_ReturnsDefault()
    {
        // Arrange
        int appointmentId = 1;
        int patientId = 2;

        // Act
        var result = await _fixture.AppointmentService.CancelAsync(appointmentId, patientId);

        // Assert
        Assert.Equal(default, result);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingAppointment_ReturnsAppointmentDto()
    {
        // Arrange
        int appointmentId = 1;

        // Act
        var result = await _fixture.AppointmentService.GetByIdAsync(appointmentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(appointmentId, result.Id);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsListOfAppointmentDto()
    {
        // Act
        var result = await _fixture.AppointmentService.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(AppointmentServiceScenarios.Appointments.Count, result.Count);
    }

    [Fact]
    public async Task GetByDoctorIdAsync_ExistingDoctor_ReturnsDoctorScheduleResponseDto()
    {
        // Arrange
        int doctorId = 1;
        int expectedAppointmentCount = AppointmentServiceScenarios.Appointments.Count(a => a.Availability.DoctorId == doctorId);

        // Act
        var result = await _fixture.AppointmentService.GetByDoctorIdAsync(doctorId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(doctorId, result.DoctorId);
        Assert.Equal(expectedAppointmentCount, result.Appointments.Count);
    }

    [Fact]
    public async Task GetByPatientIdAsync_ExistingPatient_ReturnsPatientAppointmentsResponseDto()
    {
        // Arrange
        int patientId = 1;

        // Act
        var result = await _fixture.AppointmentService.GetByPatientIdAsync(patientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(patientId, result.PatientId);
        Assert.Equal(2, result.Appointments.Count);
    }
}