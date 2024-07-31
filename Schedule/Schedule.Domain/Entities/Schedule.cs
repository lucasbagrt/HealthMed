using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Entities;

namespace Schedule.Domain.Entities
{
	public class Schedule : BaseEntity, IEntity<int>
	{
		public int DoctorId { get; set; }
		public virtual ICollection<AvailableTime> AvailableTimes { get; set; } = [];
	}
}
