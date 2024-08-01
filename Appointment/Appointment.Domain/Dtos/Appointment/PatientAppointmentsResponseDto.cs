namespace Appointment.Domain.Dtos.Appointment
{
    public class PatientAppointmentsResponseDto
    {
        public int PatientId { get; set; }
        public List<AppointmentDto>? Appointments { get; set; }
    }
}
