using AutoMapper;
using Availability.Domain.Dtos;
using Availability.Domain.Interfaces.Repositories;
using Availability.Domain.Interfaces.Services;
using Availability.Infra.Data.Context;
using Availability.Service.Validators;
using HealthMed.CrossCutting.Notifications;
using HealthMed.Domain.Dtos.Default;
using HealthMed.Service.Services;

namespace Availability.Service
{
	public class AvailabilityService(
			ApplicationDbContext context,
			IAvailabilityRepository availabilityRepository,
			IMapper mapper,
			NotificationContext notificationContext) : BaseService, IAvailabilityService
	{
		public async Task<DefaultServiceResponseDto?> AddAvailabilityAsync(AddAvailabilityDto addAvailabilityDto, int doctorId)
		{
			var validationResult = Validate(addAvailabilityDto, Activator.CreateInstance<AddAvailabilityDtoValidator>());
			if (!validationResult.IsValid)
			{
				notificationContext.AddNotifications(validationResult.Errors);
				return default;
			}

			var entity = mapper.Map<Domain.Entities.Availability>(addAvailabilityDto);

			entity.DoctorId = doctorId;
			entity.CreatedAt = DateTime.Now;

			await availabilityRepository.InsertAsync(entity);

			return new DefaultServiceResponseDto
			{
				Success = true,
				Message = StaticNotifications.AvailabilityCreated.Message
			};
		}

		public async Task<DefaultServiceResponseDto?> UpdateAvailabilityAsync(AvailabilityDto availabilityDto, int doctorId)
		{
			var validationResult = Validate(availabilityDto, Activator.CreateInstance<AvailabilityDtoValidator>());
			if (!validationResult.IsValid)
			{
				notificationContext.AddNotifications(validationResult.Errors);
				return default;
			}

			using var transaction = context.Database.BeginTransaction();



			var entity = mapper.Map<Domain.Entities.Availability>(availabilityDto);

			entity.DoctorId = doctorId;
			entity.CreatedAt = DateTime.Now;

			await availabilityRepository.InsertAsync(entity);


			transaction.Commit();

			return new DefaultServiceResponseDto
			{
				Success = true,
				Message = StaticNotifications.AvailabilityCreated.Message
			};
		}

		public async Task<ICollection<AvailabilityDto>> GetAvailabilitiesByDoctorAsync(int doctorId)
		{
			var availabilities = await availabilityRepository
				.SelectAsync();

			var filtered = availabilities
				.AsQueryable()
				.Where(a => a.DoctorId == doctorId);

			var dtos = mapper.Map<List<AvailabilityDto>>(filtered);

			return dtos;
		}
	}
}
