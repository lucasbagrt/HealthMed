using Appointment.Domain.Interfaces.Repositories;
using Appointment.Infra.Data.Context;
using HealthMed.Infra.Data.Repositories;

namespace Appointment.Data.Repositories;

public class AvailabilityRepository(ApplicationDbContext context) : BaseRepository<Domain.Entities.Availability, int, ApplicationDbContext>(context), IAvailabilityRepository
{
}
