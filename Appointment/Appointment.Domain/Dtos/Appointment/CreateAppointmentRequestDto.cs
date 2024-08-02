namespace Appointment.Domain.Dtos.Appointment
{
    public class CreateAppointmentRequestDto
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; } 
    }
}
