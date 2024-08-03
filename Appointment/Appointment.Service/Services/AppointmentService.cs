using Appointment.Domain.Dtos.Appointment;
using Appointment.Domain.Enums;
using Appointment.Domain.Interfaces.Integration;
using Appointment.Domain.Interfaces.Repositories;
using Appointment.Domain.Interfaces.Services;
using Appointment.Domain.Validators.Appointment;
using AutoMapper;
using HealthMed.CrossCutting.Notifications;
using HealthMed.CrossCutting.QueueMessenge;
using HealthMed.Domain.Dtos.Default;
using HealthMed.Service.Services;
using MassTransit;

namespace Appointment.Service.Services;

public class AppointmentService : BaseService, IAppointmentService
{
    private readonly IMapper _mapper;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IAvailabilityRepository _availabilityRepository;
    private readonly NotificationContext _notificationContext;
    private readonly IUserIntegration _userIntegration;
    private readonly IBus _bus;

    public AppointmentService(IMapper mapper, IAppointmentRepository appointmentRepository, IAvailabilityRepository availabilityRepository,
        NotificationContext notificationContext, IUserIntegration userIntegration, IBus bus)
    {
        _mapper = mapper;
        _appointmentRepository = appointmentRepository;
        _availabilityRepository = availabilityRepository;
        _notificationContext = notificationContext;
        _userIntegration = userIntegration;
        _bus = bus;
    }

    public async Task<DefaultServiceResponseDto> CreateAsync(CreateAppointmentRequestDto createAppointmentRequestDto,
            int patientId, string token)
    {        
        var validationResult = Validate(createAppointmentRequestDto, Activator.CreateInstance<CreateAppointmentValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        bool isValidAvailability = await IsValidAvailabilityAsync(createAppointmentRequestDto.AvailabilityId);
        if (!isValidAvailability)
        {
            _notificationContext.AddNotification(StaticNotifications.AvailabilityNotAvailable.Key, StaticNotifications.AvailabilityNotAvailable.Message);
            return default;
        }       

        await AppointmentQueue(createAppointmentRequestDto, patientId, token);

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.AppointmentCreated.Message
        };
    }

    public async Task<DefaultServiceResponseDto> UpdateAsync(UpdateAppointmentRequestDto updateAppointmentRequestDto, int patientId)
    {
        var validationResult = Validate(updateAppointmentRequestDto, Activator.CreateInstance<UpdateAppointmentValidator>());
        if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

        var existingAppointment = await _appointmentRepository.SelectAsync(updateAppointmentRequestDto.Id);
        if (existingAppointment == null)
        {
            _notificationContext.AddNotification(StaticNotifications.AppointmentNotFound.Key, StaticNotifications.AppointmentNotFound.Message);
            return default;
        }        

        if (existingAppointment.PatientId != patientId)
        {
            _notificationContext.AddNotification(StaticNotifications.InvalidPatient.Key, StaticNotifications.InvalidPatient.Message);
            return default;
        }        

        bool isValidAvailability = await IsValidAvailabilityAsync(updateAppointmentRequestDto.AvailabilityId);
        if (!isValidAvailability)
        {
            _notificationContext.AddNotification(StaticNotifications.AvailabilityNotAvailable.Key, StaticNotifications.AvailabilityNotAvailable.Message);
            return default;
        }        

        existingAppointment.AvailabilityId = updateAppointmentRequestDto.AvailabilityId;
        await _appointmentRepository.UpdateAsync(existingAppointment);

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.AppointmentUpdated.Message
        };
    }

    public async Task<DefaultServiceResponseDto> CancelAsync(int appointmentId, int patientId)
    {
        var existingAppointment = await _appointmentRepository.SelectAsync(appointmentId);
        if (existingAppointment == null)
        {
            _notificationContext.AddNotification(StaticNotifications.AppointmentNotFound.Key, StaticNotifications.AppointmentNotFound.Message);
            return default;
        }        

        if (existingAppointment.PatientId != patientId)
        {
            _notificationContext.AddNotification(StaticNotifications.InvalidPatient.Key, StaticNotifications.InvalidPatient.Message);
            return default;
        }        

        existingAppointment.Status = AppointmentStatus.Cancelled;
        existingAppointment.IsActive = false;

        await _appointmentRepository.UpdateAsync(existingAppointment);

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.AppointmentCancelled.Message
        };
    }

    public async Task<AppointmentDto> GetByIdAsync(int appointmentId)
    {
        var appointment = await _appointmentRepository.SelectAsync(appointmentId);
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<List<AppointmentDto>> GetAllAsync()
    {
        var appointments = await _appointmentRepository.SelectAsync();
        return _mapper.Map<List<AppointmentDto>>(appointments);
    }

    public async Task<DoctorScheduleResponseDto> GetByDoctorIdAsync(int doctorId)
    {
        var appointments = await _appointmentRepository.SelectAsync(ap => ap.Availability.DoctorId == doctorId);

        return new DoctorScheduleResponseDto
        {
            DoctorId = doctorId,
            Appointments = _mapper.Map<List<AppointmentDto>>(appointments)
        };
    }

    public async Task<PatientAppointmentsResponseDto> GetByPatientIdAsync(int patientId)
    {
        var appointments = await _appointmentRepository.SelectAsync(ap => ap.PatientId == patientId);

        return new PatientAppointmentsResponseDto
        {
            PatientId = patientId,
            Appointments = _mapper.Map<List<AppointmentDto>>(appointments)
        };
    }

    private async Task AppointmentQueue(CreateAppointmentRequestDto createAppointmentRequestDto, int patientId, string token)
    {
        var availability = await _availabilityRepository.FirstOrDefaultAsync(av => av.Id == createAppointmentRequestDto.AvailabilityId);
        var doctorUser = await _userIntegration.GetUserInfo(availability.DoctorId, token);
        var patientUser = await _userIntegration.GetUserInfo(patientId, token);

        CreateAppointment createAppointment = new()
        {
            Date = availability.Date,
            Time = availability.Time,
            DoctorId = availability.DoctorId,
            DoctorEmail = doctorUser.Email,
            DoctorName = $"{doctorUser.FirstName} {doctorUser.LastName}",
            PatientId = patientId,
            PatientName = $"{patientUser.FirstName} {patientUser.LastName}",
            AvailabilityId = availability.Id
        };

        await _bus.Publish(createAppointment);
    }

    private async Task<bool> IsValidAvailabilityAsync(int availabilityId)
    {
        var availability = await _availabilityRepository.FirstOrDefaultAsync(av => av.Id == availabilityId);
        if (availability == null || availability.Date < DateTime.Now.Date) return false;

        var hasActiveAppointment = await _appointmentRepository.SelectAsync(av => av.AvailabilityId == availabilityId &&
                                                                                  av.IsActive && av.Status != AppointmentStatus.Cancelled);
        if (hasActiveAppointment.Any()) return false;

        return true;
    }
}
