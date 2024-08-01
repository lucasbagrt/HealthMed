using Appointment.Domain.Dtos.Appointment;
using Appointment.Domain.Enums;
using Appointment.Domain.Interfaces.Integration;
using Appointment.Domain.Interfaces.Repositories;
using Appointment.Domain.Interfaces.Services;
using Appointment.Domain.Validators;
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
    private readonly IScheduleRepository _scheduleRepository;
    private readonly NotificationContext _notificationContext;
    private readonly IUserIntegration _userIntegration;
    private readonly IBus _bus;

    public AppointmentService(IMapper mapper, IAppointmentRepository appointmentRepository, IScheduleRepository scheduleRepository, NotificationContext notificationContext, IUserIntegration userIntegration, IBus bus)
    {
        _mapper = mapper;
        _appointmentRepository = appointmentRepository;
        _scheduleRepository = scheduleRepository;
        _notificationContext = notificationContext;
        _userIntegration = userIntegration;
        _bus = bus;
    }

    public async Task<DefaultServiceResponseDto> CreateAppointmentAsync(CreateAppointmentRequestDto createAppointmentRequestDto,
            int patientId, string token)
    {
        createAppointmentRequestDto.PatientId = patientId;
        var validationResult = Validate(createAppointmentRequestDto, Activator.CreateInstance<CreateAppointmentValidator>());
        if (!validationResult.IsValid)
        {
            var firstErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage;
            return new DefaultServiceResponseDto
            {
                Success = false,
                Message = firstErrorMessage ?? "Validation failed."
            };
        }

        bool existingAppointment = await _appointmentRepository.ExistsAsync(
            createAppointmentRequestDto.DoctorId,
            createAppointmentRequestDto.Date,
            createAppointmentRequestDto.Time);

        if (existingAppointment) return new DefaultServiceResponseDto() { Message = StaticNotifications.AppointmentAlreadyExists.Message, Success = false };

        await AppointmentQueue(createAppointmentRequestDto, patientId, token);

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.AppointmentCreated.Message
        };
    }

    public async Task<DefaultServiceResponseDto> UpdateAppointmentAsync(UpdateAppointmentRequestDto updateAppointmentRequestDto, int patientId)
    {
        updateAppointmentRequestDto.PatientId = patientId;
        var validationResult = Validate(updateAppointmentRequestDto, Activator.CreateInstance<UpdateAppointmentValidator>());
        if (!validationResult.IsValid)
        {
            var firstErrorMessage = validationResult.Errors.FirstOrDefault()?.ErrorMessage;
            return new DefaultServiceResponseDto
            {
                Success = false,
                Message = firstErrorMessage ?? "Validation failed."
            };
        }

        var existingAppointment = await _appointmentRepository.SelectAsync(updateAppointmentRequestDto.AppointmentId);
        if (existingAppointment == null) return new DefaultServiceResponseDto() { Message = StaticNotifications.AppointmentNotFound.Message, Success = false };
        
        if (existingAppointment.PatientId != updateAppointmentRequestDto.PatientId) return new DefaultServiceResponseDto() { Message = StaticNotifications.InvalidPatient.Message, Success = false };

        existingAppointment.Date = updateAppointmentRequestDto.Date;
        existingAppointment.Time = updateAppointmentRequestDto.Time;

        await _appointmentRepository.UpdateAsync(existingAppointment);

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.AppointmentUpdated.Message
        };
    }

    public async Task<DefaultServiceResponseDto> CancelAppointmentAsync(int appointmentId, int patientId)
    {
        var existingAppointment = await _appointmentRepository.SelectAsync(appointmentId);
        if (existingAppointment == null) return new DefaultServiceResponseDto() { Message = StaticNotifications.AppointmentNotFound.Message, Success = false };

        if (existingAppointment.PatientId != patientId) return new DefaultServiceResponseDto() { Message = StaticNotifications.InvalidPatient.Message, Success = false };

        existingAppointment.Status = AppointmentStatus.Cancelled;
        existingAppointment.IsActive = false;

        await _appointmentRepository.UpdateAsync(existingAppointment);

        return new DefaultServiceResponseDto
        {
            Success = true,
            Message = StaticNotifications.AppointmentCancelled.Message
        };
    }

    public async Task<AppointmentDto> GetAppointmentByIdAsync(int appointmentId)
    {
        var appointment = await _appointmentRepository.SelectAsync(appointmentId);

        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
    {
        var appointments = await _appointmentRepository.SelectAsync();

        return _mapper.Map<List<AppointmentDto>>(appointments);
    }

    public async Task<DoctorScheduleResponseDto> GetAppointmentsByDoctorIdAsync(int doctorId)
    {
        var appointments = await _appointmentRepository.GetAppointmentsByDoctorIdAsync(doctorId);

        return new DoctorScheduleResponseDto
        {
            DoctorId = doctorId,
            Appointments = _mapper.Map<List<AppointmentDto>>(appointments)
        };
    }

    public async Task<PatientAppointmentsResponseDto> GetAppointmentsByPatientIdAsync(int patientId)
    {
        var appointments = await _appointmentRepository.GetAppointmentsByPatientIdAsync(patientId);

        return new PatientAppointmentsResponseDto
        {
            PatientId = patientId,
            Appointments = _mapper.Map<List<AppointmentDto>>(appointments)
        };
    }

    public async Task<List<AvailableSlotDto>> GetAvailableSlotsAsync(int doctorId, DateTime date)
    {
        var appointments = await _appointmentRepository.GetAppointmentsByDoctorIdAndDateAsync(doctorId, date.Date);
        var workingHours = await _scheduleRepository.GetWorkingHoursByDoctorIdAsync(doctorId);

        var availableSlots = workingHours
            .Where(hour => !appointments.Any(a => a.Time == hour.StartTime))
            .Select(hour => new AvailableSlotDto { Time = hour.StartTime })
            .ToList();

        return availableSlots;
    }

    private async Task AppointmentQueue(CreateAppointmentRequestDto createAppointmentRequestDto, int patientId, string token)
    {
        var doctorUser = await _userIntegration.GetUserInfo(createAppointmentRequestDto.DoctorId, token);
        var patientUser = await _userIntegration.GetUserInfo(patientId, token);

        CreateAppointment createAppointment = new()
        {
            Date = createAppointmentRequestDto.Date,
            Time = createAppointmentRequestDto.Time,
            DoctorId = createAppointmentRequestDto.DoctorId,
            DoctorEmail = doctorUser.Email,
            DoctorName = $"{doctorUser.FirstName} {doctorUser.LastName}",
            PatientId = patientId,
            PatientName = $"{patientUser.FirstName} {patientUser.LastName}",
        };

        await _bus.Publish(createAppointment);
    }
}
