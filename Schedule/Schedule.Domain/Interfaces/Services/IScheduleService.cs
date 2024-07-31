using HealthMed.Domain.Dtos.Default;
using Schedule.Domain.Dtos;

namespace Schedule.Domain.Interfaces.Services
{
	public interface IScheduleService
	{
		Task<DefaultServiceResponseDto> AddScheduleAsync(AddScheduleDto addScheduleDto, int doctorId);
	}
}
