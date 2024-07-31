using HealthMed.Infra.Data.Repositories;
using Schedule.Domain.Interfaces.Repositories;
using Schedule.Infra.Data.Context;

namespace Schedule.Infra.Data.Repositories
{
    public class AvailableTimeRepository(ApplicationDbContext context) : BaseRepository<Domain.Entities.AvailableTime, int, ApplicationDbContext>(context), IAvailableTimeRepository
	{
	}
}