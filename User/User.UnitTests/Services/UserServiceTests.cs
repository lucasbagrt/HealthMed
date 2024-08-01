using HealthMed.CrossCutting.Notifications;
using User.Domain.Dtos.User;
using User.UnitTests.Fixtures;
using User.UnitTests.Scenarios;

namespace User.UnitTests.Services;

public class UserServiceTests : IClassFixture<UserServiceFixture>
{
    private readonly UserServiceFixture _fixture;

    public UserServiceTests(UserServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAllAsync_ValidFilter_ReturnsUserResponseDtoCollection()
    {
        // Arrange
        var filter = UserServiceScenarios.ValidUserFilter();

        // Act
        var result = await _fixture.UserService.GetAllAsync(filter);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<List<UserResponseDto>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetByIdAsync_ValidId_ReturnsUserResponseDto()
    {
        // Arrange
        int userId = 1;

        // Act
        var result = await _fixture.UserService.GetByIdAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UserResponseDto>(result);
        Assert.Equal(userId, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ValidUpdateUserDto_ReturnsSuccessResponse()
    {
        // Arrange
        var updateUserDto = UserServiceScenarios.ValidUpdateUserDto();
        int userId = 1;

        // Act
        var result = await _fixture.UserService.UpdateAsync(updateUserDto, userId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(StaticNotifications.UserEdited.Message, result.Message);
    }

    [Fact]
    public async Task UpdatePasswordAsync_ValidUpdateUserPasswordDto_ReturnsSuccessResponse()
    {
        // Arrange
        var updateUserPasswordDto = UserServiceScenarios.ValidUpdateUserPasswordDto();
        int userId = 1;

        // Act
        var result = await _fixture.UserService.UpdatePasswordAsync(updateUserPasswordDto, userId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(StaticNotifications.PasswordChanged.Message, result.Message);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_ReturnsSuccessResponse()
    {
        // Arrange
        int userId = 1;

        // Act
        var result = await _fixture.UserService.DeleteAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(StaticNotifications.UserDeleted.Message, result.Message);
    }

    [Fact]
    public async Task ActivateAsync_ValidActivateUserDto_ReturnsSuccessResponse()
    {
        // Arrange
        var activateUserDto = UserServiceScenarios.ValidActivateUserDto();

        // Act
        var result = await _fixture.UserService.ActivateAsync(activateUserDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(StaticNotifications.UserActivated.Message, result.Message);
    }
}