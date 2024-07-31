using AutoMapper;
using HealthMed.CrossCutting.Notifications;
using HealthMed.Domain.Dtos.Default;
using HealthMed.Service.Services;
using Schedule.Domain.Dtos;
using Schedule.Domain.Interfaces.Repositories;
using Schedule.Domain.Interfaces.Services;
using Schedule.Service.Validators;

namespace Schedule.Service
{
	public class ScheduleService(
			IScheduleRepository scheduleRepository,
			IAvailableTimeRepository availableTimeRepository,
			IMapper mapper,
			NotificationContext notificationContext) : BaseService, IScheduleService
	{
		public async Task<DefaultServiceResponseDto?> AddScheduleAsync(AddScheduleDto addScheduleDto, int doctorId)
		{
			addScheduleDto.DoctorId = doctorId;

			var validationResult = Validate(addScheduleDto, Activator.CreateInstance<AddScheduleValidator>());
			if (!validationResult.IsValid)
			{
				notificationContext.AddNotifications(validationResult.Errors);
				return default;
			}

			var entity = mapper.Map<Domain.Entities.Schedule>(addScheduleDto);

			entity.CreatedAt = DateTime.Now;

			return null;
		}
	}
}
