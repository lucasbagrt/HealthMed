namespace Availability.Domain.Dtos
{
	public class AvailabilityDto
	{
		public int DoctorId { get; set; }
		public List<AvailableTimeDto> AvailableTimes { get; set; } = [];
	}
}
