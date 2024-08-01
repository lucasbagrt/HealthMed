using HealthMed.Domain.Interfaces.Repositories;

namespace Availability.Domain.Interfaces.Repositories
{
    public interface IAvailableTimeRepository : IBaseRepository<Entities.AvailableTime, int>
    {
    }
}
