using Appointment.Domain.Interfaces.Repositories;
using Appointment.Infra.Data.Context;
using HealthMed.Infra.Data.Repositories;

namespace Appointment.Infra.Data.Repositories;

public class AppointmentRepository(ApplicationDbContext context) : BaseRepository<Domain.Entities.Appointment, int, ApplicationDbContext>(context), IAppointmentRepository
{              
}
