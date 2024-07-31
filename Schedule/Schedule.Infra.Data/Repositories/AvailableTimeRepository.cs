using HealthMed.Infra.Data.Repositories;
using Schedule.Domain.Interfaces.Repositories;
using Schedule.Infra.Data.Context;

namespace Schedule.Infra.Data.Repositories
{
    public class ScheduleRepository(ApplicationDbContext context) : BaseRepository<Domain.Entities.Schedule, int, ApplicationDbContext>(context), IScheduleRepository
	{
	}
}
