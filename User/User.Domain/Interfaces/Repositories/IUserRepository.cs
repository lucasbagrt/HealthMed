using HealthMed.Domain.Interfaces.Repositories;

namespace User.Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<Entities.User, int>
{
}
