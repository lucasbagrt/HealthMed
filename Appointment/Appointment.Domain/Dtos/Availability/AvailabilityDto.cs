namespace Appointment.Domain.Dtos.Availability;

public class AvailabilityDto
{
    public int Id { get; set; }
    public int DoctorId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
}
