using HealthMed.Domain.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;

namespace User.Domain.Entities;

public class User : IdentityUser<int>, IEntity<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool Active { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }   
    public string Crm { get; set; }
}
