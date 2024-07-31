using HealthMed.Domain.Interfaces.Repositories;

namespace Schedule.Domain.Interfaces.Repositories
{
    public interface IScheduleRepository : IBaseRepository<Entities.Schedule, int>
    {
    }
}
