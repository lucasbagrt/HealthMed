using Entities = User.Domain.Entities;
using User.Domain.Interfaces.Repositories;
using User.Infra.Data.Context;
using HealthMed.Infra.Data.Repositories;

namespace User.Infra.Data.Repositories;

public class UserRepository(ApplicationDbContext context) : BaseRepository<Entities.User, int, ApplicationDbContext>(context), IUserRepository
{
}