using HealthMed.Infra.Data.Repositories;
using Availability.Domain.Interfaces.Repositories;
using Availability.Infra.Data.Context;

namespace Availability.Infra.Data.Repositories
{
    public class AvailabilityRepository(ApplicationDbContext context) : BaseRepository<Domain.Entities.Availability, int, ApplicationDbContext>(context), IAvailabilityRepository
	{
	}
}
