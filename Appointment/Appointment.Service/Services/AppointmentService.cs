using Appointment.Domain.Interfaces.Repositories;
using Appointment.Domain.Interfaces.Services;
using AutoMapper;
using HealthMed.CrossCutting.Notifications;
using HealthMed.Service.Services;

namespace Appointment.Service.Services
{
    public class AppointmentService(IMapper mapper, IAppointmentRepository appointmentRepository, NotificationContext notificationContext) : BaseService, IAppointmentService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
        private readonly NotificationContext _notificationContext = notificationContext;
    {
    }
}
