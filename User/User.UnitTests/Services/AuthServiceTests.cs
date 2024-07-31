using HealthMed.CrossCutting.Notifications;
using User.UnitTests.Fixtures;
using User.UnitTests.Scenarios;

namespace User.UnitTests.Services;

public class AuthServiceTests : IClassFixture<AuthServiceFixture>
{
    private readonly AuthServiceFixture _fixture;

    public AuthServiceTests(AuthServiceFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsLoginResponse()
    {
        // Arrange
        var loginDto = AuthServiceScenarios.ValidLoginDto();

        // Act
        var result = await _fixture.AuthService.LoginAsync(loginDto);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.AccessToken);
        Assert.NotEmpty(result.RefreshToken);
        Assert.NotEqual(default, result.Expires);
    }

    [Fact]
    public async Task LoginAsync_InvalidCredentials_ReturnsNull()
    {
        // Arrange
        var loginDto = AuthServiceScenarios.InvalidLoginDto();

        // Act
        var result = await _fixture.AuthService.LoginAsync(loginDto);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task RegisterAsync_ValidUser_ReturnsSuccess()
    {
        // Arrange
        var registerDto = AuthServiceScenarios.ValidRegisterDto();

        // Act
        var result = await _fixture.AuthService.RegisterAsync(registerDto, false);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(StaticNotifications.UserCreated.Message, result.Message);
    }

    [Fact]
    public async Task RegisterAsync_ExistingUser_ReturnsDefault()
    {
        // Arrange
        var registerDto = AuthServiceScenarios.ExistingUserRegisterDto();

        // Act
        var result = await _fixture.AuthService.RegisterAsync(registerDto, false);

        // Assert
        Assert.Equal(default, result);
    }

    [Fact]
    public async Task RevokeAsync_ValidUser_ReturnsSuccess()
    {
        // Arrange
        var userName = "validuser";

        // Act
        var result = await _fixture.AuthService.RevokeAsync(userName);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(StaticNotifications.RevokeToken.Message, result.Message);
    }
}