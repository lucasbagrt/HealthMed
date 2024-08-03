using Appointment.UnitTests.Fixtures;
using Appointment.UnitTests.Scenarios;
using HealthMed.CrossCutting.Notifications;

namespace Appointment.UnitTests.Services;

public class AvailabilityServiceTests : IClassFixture<AvailabilityServiceFixture>
{
    private readonly AvailabilityServiceFixture _fixture;

    public AvailabilityServiceTests(AvailabilityServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateAsync_ValidAvailability_ReturnsSuccess()
    {
        // Arrange
        var createAvailabilityDto = AvailabilityServiceScenarios.ValidCreateAvailabilityDto();
        int doctorId = 1;

        // Act
        var result = await _fixture.AvailabilityService.CreateAsync(createAvailabilityDto, doctorId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(StaticNotifications.AvailabilityCreated.Message, result.Message);
    }

    [Fact]
    public async Task DeleteAsync_ValidAvailability_ReturnsSuccess()
    {
        // Arrange
        int availabilityId = 1;
        int doctorId = 1;

        // Act
        var result = await _fixture.AvailabilityService.DeleteAsync(availabilityId, doctorId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(StaticNotifications.AvailabilityDeleted.Message, result.Message);
    }

    [Fact]
    public async Task GetByDoctorAsync_ExistingDoctor_ReturnsAvailabilities()
    {
        // Arrange
        int doctorId = 1;

        // Act
        var result = await _fixture.AvailabilityService.GetByDoctorAsync(doctorId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task UpdateAsync_ValidAvailability_ReturnsSuccess()
    {
        // Arrange
        var updateAvailabilityDto = AvailabilityServiceScenarios.ValidUpdateAvailabilityDto();
        int doctorId = 1;

        // Act
        var result = await _fixture.AvailabilityService.UpdateAsync(updateAvailabilityDto, doctorId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(StaticNotifications.AvailabilityChanged.Message, result.Message);
    }
}