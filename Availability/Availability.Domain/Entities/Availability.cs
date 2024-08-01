using HealthMed.Domain.Entities;
using HealthMed.Domain.Interfaces.Entities;

namespace Availability.Domain.Entities
{
	public class Availability : BaseEntity, IEntity<int>
	{
		public int DoctorId { get; set; }
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
	}
}
