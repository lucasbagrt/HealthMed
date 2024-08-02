using Appointment.Domain.Dtos.Appointment;
using Appointment.Domain.Enums;
using Appointment.Domain.Interfaces.Repositories;
using Appointment.Domain.Interfaces.Services;
using Appointment.Domain.Validators;
using AutoMapper;
using HealthMed.CrossCutting.Notifications;
using HealthMed.Domain.Dtos.Default;
using HealthMed.Service.Services;

namespace Appointment.Service.Services
{
    public class AppointmentService : BaseService, IAppointmentService
{
    private readonly IMapper _mapper;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly NotificationContext _notificationContext;

    public AppointmentService(IMapper mapper, IAppointmentRepository appointmentRepository, IScheduleRepository scheduleRepository, NotificationContext notificationContext)
    {
        _mapper = mapper;
        _appointmentRepository = appointmentRepository;
        _scheduleRepository = scheduleRepository;
        _notificationContext = notificationContext;
    }

    public async Task<DefaultServiceResponseDto> CreateAppointmentAsync(CreateAppointmentRequestDto createAppointmentRequestDto,
            int patientId)
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

            var appointment = _mapper.Map<Domain.Entities.Appointment>(createAppointmentRequestDto);

            appointment.Status = AppointmentStatus.Scheduled;
            appointment.IsActive = true;

            await _appointmentRepository.InsertAsync(appointment);

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

        public async Task<PatientAppointmentsResponseDto> GetAppointmentsByPatientIdAsync(int patientId)
        {
            var appointments = await _appointmentRepository.SelectAsync();
            var patientAppointments = appointments
                .Where(p => p.PatientId == patientId && p.IsActive)
                .ToList();

            return new PatientAppointmentsResponseDto
            {
                PatientId = patientId,
                Appointments = _mapper.Map<List<AppointmentDto>>(patientAppointments)
            };
        }

        public async Task<DoctorScheduleResponseDto> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            var appointments = await _appointmentRepository.SelectAsync();
            var doctorAppointments = appointments
                .Where(p => p.DoctorId == doctorId && p.IsActive)
                .ToList();

            return new DoctorScheduleResponseDto
            {
                DoctorId = doctorId,
                Appointments = _mapper.Map<List<AppointmentDto>>(doctorAppointments)
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
    }
}
