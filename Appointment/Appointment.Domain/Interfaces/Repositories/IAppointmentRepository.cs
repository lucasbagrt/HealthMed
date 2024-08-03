﻿using HealthMed.Domain.Interfaces.Repositories;

namespace Appointment.Domain.Interfaces.Repositories;

public interface IAppointmentRepository : IBaseRepository<Entities.Appointment, int>
{               
}
