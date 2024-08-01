using HealthMed.Domain.Entities;
using HealthMed.Domain.Utilities;
using User.Domain.Dtos.Auth;
using User.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace User.Infra.Data.Seeds._SeedHistory;

public class Seed_20240426152400_Add_Admin : Seed
{
    private readonly IAuthService authService;

    public Seed_20240426152400_Add_Admin(DbContext dbContext, IServiceProvider serviceProvider) : base(dbContext)
    {
        authService = serviceProvider.CreateScope().ServiceProvider.GetService<IAuthService>();
    }

    public override void Up()
    {
        var user = new RegisterDto
        {
            FirstName = "Admin",            
            Email = "admin@gmail.com",
            Password = "1q2w3e4r@#$A",
            LastName = "Admin",
            Username = "admin",
            Role = Domain.Enums.Role.ADMIN
        };

        authService.RegisterAsync(user, true).Wait();
    }
}
