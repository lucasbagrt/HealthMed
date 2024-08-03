using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Entities;

namespace Appointment.Domain.Entities;

public class Availability : BaseEntity, IEntity<int>
{
    public int DoctorId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }    
}