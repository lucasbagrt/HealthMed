using Availability.UnitTests.Fixtures;

namespace Availability.UnitTests.Services
{
    public class AvailabilityServiceTests(AvailabilityServiceFixture fixture) : IClassFixture<AvailabilityServiceFixture>
    {
        [Fact]
		public async Task GetAvailabilities_ValidDoctorId_ReturnsAvailabilitiesResponse()
		{
			// Arrange
			var doctorId = 1;

			// Act
			var result = await fixture.AvailabilityService.GetAvailabilitiesByDoctorAsync(doctorId);

			// Assert
			Assert.NotEmpty(result);
		}

		[Fact]
		public async Task GetAvailabilities_InvalidDoctorId_ReturnsEmptyResponse()
		{
			// Arrange
			var doctorId = -1;

			// Act
			var result = await fixture.AvailabilityService.GetAvailabilitiesByDoctorAsync(doctorId);

			// Assert
			Assert.Empty(result);
		}
	}
}