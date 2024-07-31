using Appointment.Domain;
using Appointment.Domain.Dtos.Appointment;
using Appointment.Domain.Interfaces.Repositories;
using Appointment.Domain.Interfaces.Services;
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
            if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; }

            bool existingAppointment = await _appointmentRepository.ExistsAsync(
              createAppointmentRequestDto.DoctorId,
              createAppointmentRequestDto.Date,
              createAppointmentRequestDto.Time);

            if (existingAppointment)
            {
                _notificationContext.AddNotification(StaticNotifications.AppointmentAlreadyExists);
                return default;
            }

            var appointment = _mapper.Map<Domain.Entities.Appointment>(createAppointmentRequestDto);
            appointment.Status = AppointmentStatusEnum.Scheduled;

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
            if (!validationResult.IsValid) { _notificationContext.AddNotifications(validationResult.Errors); return default; };

            var existingAppointment = await _appointmentRepository.SelectAsync(updateAppointmentRequestDto.AppointmentId);
            if (existingAppointment == null)
            {
                _notificationContext.AddNotification(StaticNotifications.AppointmentNotFound);
                return default;
            }
            else if (existingAppointment.PatientId != updateAppointmentRequestDto.PatientId)
            {
                _notificationContext.AddNotification(StaticNotifications.InvalidPatient);
                return default;
            }

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
            if (existingAppointment == null)
            {
                _notificationContext.AddNotification(StaticNotifications.AppointmentNotFound);
                return default;
            }
            else if (existingAppointment.PatientId != patientId)
            {
                _notificationContext.AddNotification(StaticNotifications.InvalidPatient);
                return default;
            }

            existingAppointment.Status = AppointmentStatusEnum.Cancelled;
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
            if (appointment == null)
            {
                _notificationContext.AddNotification(StaticNotifications.AppointmentNotFound);
                return default;
            }

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
            var availableSlots = new List<AvailableSlotDto>();

            foreach (var hour in workingHours)
            {
                if (!appointments.Any(a => a.Time == hour.StartTime))
                {
                    availableSlots.Add(new AvailableSlotDto { Time = hour.StartTime });
                }
            }

            return availableSlots;
        }
    }
}
