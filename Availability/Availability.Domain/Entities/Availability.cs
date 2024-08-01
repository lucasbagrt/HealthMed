using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Entities;

namespace Availability.Domain.Entities
{
	public class Availability : BaseEntity, IEntity<int>
	{
		public int DoctorId { get; set; }
		public virtual ICollection<AvailableTime> AvailableTimes { get; set; } = [];
	}
}
