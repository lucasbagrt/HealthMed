using Appointment.Domain.Enums;

namespace Appointment.Domain.Dtos.Appointment
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
