using User.Domain.Dtos.Auth;

namespace User.UnitTests.Scenarios;

public static class AuthServiceScenarios
{
    public static LoginDto ValidLoginDto()
    {
        return new LoginDto
        {
            Username = "validuser",
            Password = "validpassword"
        };
    }

    public static LoginDto InvalidLoginDto()
    {
        return new LoginDto
        {
            Username = "invaliduser",
            Password = "invalidpassword"
        };
    }

    public static RegisterDto ValidRegisterDto()
    {
        return new RegisterDto
        {
            FirstName = "newuser",            
            Username = "newuser",
            Password = "1q2w3e4r@#$A",
            Email = "newuser@example.com",
            Role = Domain.Enums.Role.USER
        };
    }

    public static RegisterDto ExistingUserRegisterDto()
    {
        return new RegisterDto
        {
            Username = "existinguser",
            Password = "existingpassword",
            Email = "existinguser@example.com",
            Role = Domain.Enums.Role.USER
        };
    }
}