using HealthMed.Domain.Dtos.Default;
using Availability.Domain.Dtos;

namespace Availability.Domain.Interfaces.Services
{
	public interface IAvailabilityService
	{
		Task<DefaultServiceResponseDto?> AddAvailabilityAsync(AddAvailabilityDto addAvailabilityDto, int doctorId);
		Task<DefaultServiceResponseDto?> UpdateAvailabilityAsync(AvailabilityDto addAvailabilityDto, int doctorId);
		Task<ICollection<AvailabilityDto>> GetAvailabilitiesByDoctorAsync(int doctorId);
	}
}
