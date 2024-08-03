namespace Appointment.Domain.Dtos.Availability;

public class UpdateAvailabilityDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
}
