using Appointment.Domain.Dtos.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment.Domain.Interfaces.Repositories
{
    public interface IScheduleRepository
    {
        Task<List<WorkingHourDto>> GetWorkingHoursByDoctorIdAsync(int doctorId);
    }
}
