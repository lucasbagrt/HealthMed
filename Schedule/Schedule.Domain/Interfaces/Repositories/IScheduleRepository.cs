using HealthMed.Domain.Interfaces.Repositories;

namespace Schedule.Domain.Interfaces.Repositories
{
    public interface IAvailableTimeRepository : IBaseRepository<Entities.AvailableTime, int>
    {
    }
}
