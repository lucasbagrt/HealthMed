using Appointment.Domain.Enums;
using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Entities;

namespace Appointment.Domain.Entities;

public class Appointment : BaseEntity, IEntity<int>
{    
    public int PatientId { get; set; }
    public int AvailabilityId { get; set; }
    public AppointmentStatus Status { get; set; }
    public bool IsActive { get; set; }
    public virtual Availability Availability { get; set; }
}