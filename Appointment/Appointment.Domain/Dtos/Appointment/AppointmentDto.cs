using Appointment.Domain.Dtos.Availability;
using Appointment.Domain.Enums;

namespace Appointment.Domain.Dtos.Appointment;

public class AppointmentDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int AvailabilityId { get; set; }
    public AppointmentStatus Status { get; set; }
    public AvailabilityDto Availability { get; set; }
}
