using Availability.Domain.Dtos;
using HealthMed.Domain.Dtos.Default;

namespace Availability.Domain.Interfaces.Services
{
	public interface IAvailabilityService
	{
		Task<DefaultServiceResponseDto?> AddAvailabilityAsync(List<AvailabilityDto> listAvailabilityDto, int doctorId);
		Task<DefaultServiceResponseDto?> UpdateAvailabilityAsync(List<AvailabilityDto> listAvailabilityDto, int doctorId);
		Task<ICollection<AvailabilityDto>> GetAvailabilitiesByDoctorAsync(int doctorId);
	}
}
