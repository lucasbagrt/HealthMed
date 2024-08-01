namespace Appointment.Domain.Dtos.Appointment
{
    public class DoctorScheduleResponseDto
    {
        public int DoctorId { get; set; }
        public List<AppointmentDto>? Appointments { get; set; }
    }
}
