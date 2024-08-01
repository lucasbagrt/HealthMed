using HealthMed.Domain.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;

namespace User.Domain.Entities;

public class Role : IdentityRole<int>, IEntity<int>
{
    public Role(string roleName)
    {
        Name = roleName;
        NormalizedName = roleName;
    }
    public Role() { }
}
