using Appointment.Domain.Dtos.Appointment;
using Appointment.Domain.Interfaces.Repositories;

namespace Appointment.Data.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        public async Task<List<WorkingHourDto>> GetWorkingHoursByDoctorIdAsync(int doctorId)
        {
            // simulação
            return await Task.FromResult(new List<WorkingHourDto>
            {
                new WorkingHourDto { StartTime = TimeSpan.FromHours(9), EndTime = TimeSpan.FromHours(12) },
                new WorkingHourDto { StartTime = TimeSpan.FromHours(13), EndTime = TimeSpan.FromHours(17) }
            });
        }
    }
}
