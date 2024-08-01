using AutoMapper;
using HealthMed.CrossCutting.Notifications;
using HealthMed.Domain.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using User.Domain.Dtos.Auth;
using User.Service.Services;
using Entities = User.Domain.Entities;

namespace User.UnitTests.Fixtures;

public class AuthServiceFixture
{
    public AuthService AuthService { get; }

    public AuthServiceFixture()
    {
        var userManagerMock = new Mock<UserManager<Entities.User>>(Mock.Of<IUserStore<Entities.User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        var configurationMock = new Mock<IConfiguration>();
        var mapperMock = new Mock<IMapper>();
        var notificationContextMock = new Mock<NotificationContext>();

        // Cenário: Login com credenciais válidas
        var validUser = new Entities.User
        {
            UserName = "validuser",
            Active = true,
            RefreshToken = "valid_refresh_token",
            RefreshTokenExpiryTime = DateTime.Now.AddHours(1),
        };
        userManagerMock.Setup(x => x.FindByNameAsync("validuser")).ReturnsAsync(validUser);
        userManagerMock.Setup(x => x.CheckPasswordAsync(validUser, "validpassword")).ReturnsAsync(true);
        userManagerMock.Setup(x => x.GetRolesAsync(validUser)).ReturnsAsync(new List<string> { StaticUserRoles.ADMIN });

        // Cenário: Login com credenciais inválidas
        userManagerMock.Setup(x => x.FindByNameAsync("invaliduser")).ReturnsAsync((Entities.User)null!);

        // Cenário: Registro de usuário válido
        userManagerMock.Setup(x => x.FindByNameAsync("newuser")).ReturnsAsync((Entities.User)null!);
        userManagerMock.Setup(x => x.CreateAsync(It.IsAny<Entities.User>(), "1q2w3e4r@#$A")).ReturnsAsync(IdentityResult.Success);
        userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<Entities.User>(), "USER")).ReturnsAsync(IdentityResult.Success);

        // Cenário: Registro de usuário existente
        var existingUser = new Entities.User { UserName = "existinguser", Active = true };
        userManagerMock.Setup(x => x.FindByNameAsync("existinguser")).ReturnsAsync(existingUser);

        configurationMock.Setup(x => x["JWT:Secret"]).Returns("82d6a294c62a497eb9646191a4fe0450");
        configurationMock.Setup(x => x["JWT:TokenValidityInHours"]).Returns("1");
        configurationMock.Setup(x => x["JWT:ValidIssuer"]).Returns("https://localhost:7019");
        configurationMock.Setup(x => x["JWT:ValidAudience"]).Returns("https://localhost:3000");
        configurationMock.Setup(x => x["JWT:RefreshTokenValidityInHours"]).Returns("1");

        mapperMock.Setup(x => x.Map<Entities.User>(It.IsAny<RegisterDto>()))
                  .Returns((RegisterDto dto) => new Entities.User
                  {
                      UserName = dto.Username,
                      Email = dto.Email,
                      FirstName = dto.FirstName,
                      LastName = dto.LastName,
                      PhoneNumber = dto.PhoneNumber
                  });

        AuthService = new AuthService(
            userManagerMock.Object,
            configurationMock.Object,
            mapperMock.Object,
            notificationContextMock.Object);
    }
}