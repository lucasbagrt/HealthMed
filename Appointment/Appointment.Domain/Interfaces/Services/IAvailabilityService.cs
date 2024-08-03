using Appointment.Domain.Dtos.Availability;
using HealthMed.Domain.Dtos.Default;

namespace Appointment.Domain.Interfaces.Services;

public interface IAvailabilityService
{
    Task<DefaultServiceResponseDto> CreateAsync(CreateAvailabilityDto createAvailabilityDto, int doctorId);
    Task<DefaultServiceResponseDto> UpdateAsync(UpdateAvailabilityDto updateAvailabilityDto, int doctorId);
    Task<ICollection<AvailabilityDto>> GetByDoctorAsync(int doctorId);
    Task<DefaultServiceResponseDto> DeleteAsync(int availabilityId, int doctorId);
}
