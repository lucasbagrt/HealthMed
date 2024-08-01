using HealthMed.Infra.Data.Repositories;
using Availability.Domain.Interfaces.Repositories;
using Availability.Infra.Data.Context;

namespace Availability.Infra.Data.Repositories
{
    public class AvailableTimeRepository(ApplicationDbContext context) : BaseRepository<Domain.Entities.AvailableTime, int, ApplicationDbContext>(context), IAvailableTimeRepository
	{
	}
}