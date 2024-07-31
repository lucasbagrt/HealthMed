using User.Domain.Enums;

namespace User.Domain.Dtos.Auth;

public class RegisterDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }    
    public string Crm { get; set; }    
    public string Password { get; set; }
    public Role Role { get; set; }
}
