namespace Schedule.Domain.Dtos
{
	public class AddScheduleDto
	{
		public int DoctorId { get; set; }
		public List<AvailableTimeDto> AvailableTimes { get; set; } = [];
	}
}
