namespace Appointment.Domain.Dtos.Availability;

public class CreateAvailabilityDto
{    
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
}
