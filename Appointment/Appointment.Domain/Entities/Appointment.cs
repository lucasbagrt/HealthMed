using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Entities;

namespace Appointment.Domain.Entities
{
    public class Appointment : BaseEntity, IEntity<int>
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; } 
        public string Status { get; set; }
        public bool IsActive { get; set; }
    }

}
