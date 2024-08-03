using Appointment.Domain.Dtos.Appointment;
using HealthMed.Domain.Dtos.Default;

namespace Appointment.Domain.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<DefaultServiceResponseDto> CancelAsync(int appointmentId, int patientId);
        Task<DefaultServiceResponseDto> CreateAsync(CreateAppointmentRequestDto createAppointmentRequestDto,int patientId, string token);
        Task<List<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto> GetByIdAsync(int appointmentId);
        Task<DoctorScheduleResponseDto> GetByDoctorIdAsync(int doctorId);
        Task<PatientAppointmentsResponseDto> GetByPatientIdAsync(int patientId);        
        Task<DefaultServiceResponseDto> UpdateAsync(UpdateAppointmentRequestDto updateAppointmentRequestDto, int patientId);
    }
}
