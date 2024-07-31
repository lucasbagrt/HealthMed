using Appointment.Domain.Interfaces.Repositories;
using Appointment.Infra.Data.Context;
using HealthMed.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Infra.Data.Repositories
{
    public class AppointmentRepository(ApplicationDbContext context) : BaseRepository<Domain.Entities.Appointment, int, ApplicationDbContext>(context), IAppointmentRepository
    {
        public async Task<bool> ExistsAsync(int doctorId, DateTime date, TimeSpan time)
        {
            return await _dataContext.Appointments
                .AnyAsync(a => a.DoctorId == doctorId && a.Date == date && a.Time == time);
        }

        public async Task<IList<Domain.Entities.Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            return await _dataContext.Set<Domain.Entities.Appointment>()
                .Where(a => a.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IList<Domain.Entities.Appointment>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            return await _dataContext.Set<Domain.Entities.Appointment>()
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<IList<Domain.Entities.Appointment>> GetAppointmentsByDoctorIdAndDateAsync(int doctorId, DateTime date)
        {
            return await _dataContext.Appointments
                .Where(a => a.DoctorId == doctorId && a.Date.Date == date.Date)
                .ToListAsync();
        }
    }
}
