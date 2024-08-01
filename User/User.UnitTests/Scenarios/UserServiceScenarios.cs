using User.Domain.Dtos.User;
using User.Domain.Filters;

namespace User.UnitTests.Scenarios;

public static class UserServiceScenarios
{
    public static UserFilter ValidUserFilter()
    {
        return new UserFilter
        {
            FirstName = "John",
            LastName = "Doe",
            Active = true
        };
    }

    public static UpdateUserDto ValidUpdateUserDto()
    {
        return new UpdateUserDto
        {
            Username = "johndoe",
            FirstName = "John",
            LastName = "Doe",
            Email = "john@gmail.com",
            Document = "123"
        };
    }

    public static UpdateUserPasswordDto ValidUpdateUserPasswordDto()
    {
        return new UpdateUserPasswordDto
        {
            CurrentPassword = "currentpassword",
            NewPassword = "1q2w3e4r@#$A"
        };
    }

    public static ActivateUserDto ValidActivateUserDto()
    {
        return new ActivateUserDto
        {
            Id = 1,
            Active = true
        };
    }
}