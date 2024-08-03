using Appointment.Domain.Dtos.Availability;
using Appointment.Domain.Entities;
using Appointment.Domain.Interfaces.Repositories;
using Appointment.Domain.Interfaces.Services;
using Appointment.Service.Validators.Availability;
using AutoMapper;
using HealthMed.CrossCutting.Notifications;
using HealthMed.Domain.Dtos.Default;
using HealthMed.Service.Services;

namespace Appointment.Service.Services;

public class AvailabilityService : BaseService, IAvailabilityService
{
    private readonly IMapper _mapper;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IAvailabilityRepository _availabilityRepository;    
    private readonly NotificationContext _notificationContext;

    public AvailabilityService(IMapper mapper, IAppointmentRepository appointmentRepository,
        NotificationContext notificationContext, IAvailabilityRepository availabilityRepository)
    {
        _mapper = mapper;
        _appointmentRepository = appointmentRepository;        
        _notificationContext = notificationContext;
        _availabilityRepository = availabilityRepository;
    }

    public async Task<DefaultServiceResponseDto> CreateAsync(CreateAvailabilityDto createAvailabilityDto, int doctorId)
    {
        var validationResult = Validate(createAvailabilityDto, Activator.CreateInstance<CreateAvailabilityValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        var existsAvailability = await ExistsAvailabilityByDateTime(createAvailabilityDto.Date, createAvailabilityDto.Time, doctorId);
        if (existsAvailability)
        {
            _notificationContext.AddNotification(StaticNotifications.AvailabilityAlreadyExists.Key,
            StaticNotifications.AvailabilityAlreadyExists.Message); return default;
        }

        var availability = _mapper.Map<Availability>(createAvailabilityDto);
        availability.DoctorId = doctorId;
        availability.CreatedAt = DateTime.Now;
        await _availabilityRepository.InsertAsync(availability);

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.AvailabilityCreated.Message
        };
    }

    public async Task<DefaultServiceResponseDto> DeleteAsync(int availabilityId, int doctorId)
    {
        var availability = await _availabilityRepository.SelectAsync(availabilityId);
        if (availability == null)
        {
            _notificationContext.AddNotification(StaticNotifications.AvailabilityNotFound.Key, StaticNotifications.AvailabilityNotFound.Message);
            return default;
        }

        if (availability.DoctorId != doctorId)
        {
            _notificationContext.AddNotification(StaticNotifications.AvailabilityDoesNotBelongToDoctor.Key, StaticNotifications.AvailabilityDoesNotBelongToDoctor.Message);
            return default;
        }

        await _availabilityRepository.DeleteAsync(availability.Id);

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.AvailabilityDeleted.Message
        };
    }

    public async Task<ICollection<AvailabilityDto>> GetByDoctorAsync(int doctorId)
    {
        var availabilities = await _availabilityRepository.SelectAsync(av => av.DoctorId == doctorId);
        return _mapper.Map<List<AvailabilityDto>>(availabilities);
    }

    public async Task<DefaultServiceResponseDto> UpdateAsync(UpdateAvailabilityDto updateAvailabilityDto, int doctorId)
    {
        var validationResult = Validate(updateAvailabilityDto, Activator.CreateInstance<UpdateAvailabilityValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        var availability = await _availabilityRepository.SelectAsync(updateAvailabilityDto.Id);
        if (availability == null)
        {
            _notificationContext.AddNotification(StaticNotifications.AvailabilityNotFound.Key, StaticNotifications.AvailabilityNotFound.Message);
            return default;
        }

        var existsAvailability = await ExistsAvailabilityByDateTime(updateAvailabilityDto.Date, updateAvailabilityDto.Time, doctorId);
        if (existsAvailability)
        {
            _notificationContext.AddNotification(StaticNotifications.AvailabilityAlreadyExists.Key, StaticNotifications.AvailabilityAlreadyExists.Message); 
            return default;
        }

        availability.Time = updateAvailabilityDto.Time;
        availability.Date = updateAvailabilityDto.Date;       
        await _availabilityRepository.UpdateAsync(availability);

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.AvailabilityChanged.Message
        };
    }

    private async Task<bool> ExistsAvailabilityByDateTime(DateTime date, TimeSpan time, int doctorId)
    {
        var availability = await _availabilityRepository.FirstOrDefaultAsync(av => av.DoctorId == doctorId && av.Date == date && av.Time == time);
        if (availability == null) return false;

        return true;
    }
}
