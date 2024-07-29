using HealthMed.Domain.Entities;
using HealthMed.Domain.Utilities;
using User.Domain.Entities;
using User.Infra.Data.Context;

namespace User.Infra.Data.Seeds._SeedHistory;

public class Seed_20240426140200_Add_Roles : Seed
{
    private readonly ApplicationDbContext _dbContext;
    public Seed_20240426140200_Add_Roles(ApplicationDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Up()
    {
        var roles = new List<Role>
        {
            new(StaticUserRoles.USER),
            new(StaticUserRoles.ADMIN),
            new(StaticUserRoles.DOCTOR)
        };

        _dbContext.Roles.AddRange(roles);
        _dbContext.SaveChanges();
    }
}
