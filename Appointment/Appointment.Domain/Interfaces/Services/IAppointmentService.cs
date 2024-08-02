using Appointment.Domain.Dtos.Appointment;
using HealthMed.Domain.Dtos.Default;

namespace Appointment.Domain.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<DefaultServiceResponseDto> CancelAppointmentAsync(int appointmentId, int patientId);
        Task<DefaultServiceResponseDto> CreateAppointmentAsync(CreateAppointmentRequestDto createAppointmentRequestDto,int patientId);
        Task<AppointmentDto> GetAppointmentByIdAsync(int appointmentId);
        Task<DoctorScheduleResponseDto> GetAppointmentsByDoctorIdAsync(int doctorId);
        Task<PatientAppointmentsResponseDto> GetAppointmentsByPatientIdAsync(int patientId);
        Task<List<AvailableSlotDto>> GetAvailableSlotsAsync(int doctorId, DateTime date);
        Task<DefaultServiceResponseDto> UpdateAppointmentAsync(UpdateAppointmentRequestDto updateAppointmentRequestDto, int patientId);
    }
}
