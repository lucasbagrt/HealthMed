using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Entities;

namespace Schedule.Domain.Entities
{
	public class AvailableTime : BaseEntity, IEntity<int>
	{
		public int ScheduleId { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public virtual Schedule Schedule { get; set; } = new();
	}
}
