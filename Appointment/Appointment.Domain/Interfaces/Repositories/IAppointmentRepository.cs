using Appointment.Domain.Dtos.Appointment;
using HealthMed.Domain.Interfaces.Repositories;

namespace Appointment.Domain.Interfaces.Repositories
{
    public interface IAppointmentRepository : IBaseRepository<Entities.Appointment, int>
    {
        Task<bool> ExistsAsync(int doctorId, DateTime date, TimeSpan time);
        Task<IList<Entities.Appointment>> GetAppointmentsByDoctorIdAndDateAsync(int doctorId, DateTime date);
        Task<IList<Entities.Appointment>> GetAppointmentsByDoctorIdAsync(int doctorId);
        Task<IList<Entities.Appointment>> GetAppointmentsByPatientIdAsync(int patientId);
    }
}
