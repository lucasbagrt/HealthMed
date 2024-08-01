using AutoMapper;
using HealthMed.CrossCutting.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using User.Domain.Dtos.User;
using User.Domain.Interfaces.Repositories;
using User.Service.Services;
using Entities = User.Domain.Entities;

namespace User.UnitTests.Fixtures;

public class UserServiceFixture
{
    public UserService UserService { get; }

    public UserServiceFixture()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var userManagerMock = new Mock<UserManager<Entities.User>>(Mock.Of<IUserStore<Entities.User>>(), null, null, null, null, null, null, null, null);
        var mapperMock = new Mock<IMapper>();
        var notificationContextMock = new Mock<NotificationContext>();
        var configurationMock = new Mock<IConfiguration>();

        // Cenário: GetAllAsync com filtro válido
        var users = new List<Entities.User>
        {
            new Entities.User { Id = 1, FirstName = "John", LastName = "Doe", Active = true },
            new Entities.User { Id = 2, FirstName = "Jane", LastName = "Doe", Active = true }
        };
        userRepositoryMock.Setup(x => x.SelectAsync()).ReturnsAsync(users);
        mapperMock.Setup(x => x.Map<List<UserResponseDto>>(It.IsAny<IEnumerable<Entities.User>>()))
            .Returns((IEnumerable<Entities.User> users) => users.Select(u => new UserResponseDto { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName }).ToList());

        userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                        .ReturnsAsync((string id) => users.FirstOrDefault(u => u.Id.ToString() == id));

        userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<Entities.User>()))
            .ReturnsAsync(new List<string> { "USER" });

        // Cenário: GetByIdAsync com ID válido
        var user = new Entities.User { Id = 1, FirstName = "John", LastName = "Doe" };
        userRepositoryMock.Setup(x => x.SelectAsync(1)).ReturnsAsync(user);
        mapperMock.Setup(x => x.Map<UserResponseDto>(user)).Returns(new UserResponseDto { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName });

        // Cenário: UpdateAsync com UpdateUserDto válido
        userManagerMock.Setup(x => x.FindByNameAsync("johndoe")).ReturnsAsync((Entities.User)null!);
        userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
        userManagerMock.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

        // Cenário: UpdatePasswordAsync com UpdateUserPasswordDto válido
        userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
        userManagerMock.Setup(x => x.ChangePasswordAsync(user, "currentpassword", "1q2w3e4r@#$A")).ReturnsAsync(IdentityResult.Success);

        // Cenário: DeleteAsync com ID válido
        userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
        userManagerMock.Setup(x => x.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

        // Cenário: ActivateAsync com ActivateUserDto válido
        userManagerMock.Setup(x => x.FindByIdAsync("1")).ReturnsAsync(user);
        userManagerMock.Setup(x => x.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

        UserService = new UserService(
            userRepositoryMock.Object,
            userManagerMock.Object,
            mapperMock.Object,
            notificationContextMock.Object,
            configurationMock.Object);
    }
}