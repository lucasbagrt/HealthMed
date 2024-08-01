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
		public async Task<DefaultServiceResponseDto?> AddAvailabilityAsync(List<AvailabilityDto> listAvailabilityDto, int doctorId)
		{
			var validationResult = Validate(listAvailabilityDto, Activator.CreateInstance<ListAvailabilityDtoValidator>());
			if (!validationResult.IsValid)
			{
				notificationContext.AddNotifications(validationResult.Errors);
				return default;
			}

			var entities = mapper.Map<List<Domain.Entities.Availability>>(listAvailabilityDto);

			using var transaction = context.Database.BeginTransaction();

			foreach (var entity in entities)
			{
				entity.DoctorId = doctorId;
				entity.CreatedAt = DateTime.Now;
				await availabilityRepository.InsertAsync(entity);
			}

			transaction.Commit();

			return new DefaultServiceResponseDto
			{
				Success = true,
				Message = StaticNotifications.AvailabilityCreated.Message
			};
		}

		public async Task<DefaultServiceResponseDto?> UpdateAvailabilityAsync(List<AvailabilityDto> listAvailabilityDto, int doctorId)
		{
			var validationResult = Validate(listAvailabilityDto, Activator.CreateInstance<ListAvailabilityDtoValidator>());
			if (!validationResult.IsValid)
			{
				notificationContext.AddNotifications(validationResult.Errors);
				return default;
			}

			using var transaction = context.Database.BeginTransaction();

			var existing = (await availabilityRepository.SelectAsync())
				.AsQueryable()
				.Where(a => a.DoctorId == doctorId);

			foreach (var entity in existing)
			{
				await availabilityRepository.DeleteAsync(entity.Id);
			}

			var entities = mapper.Map<List<Domain.Entities.Availability>>(listAvailabilityDto);

			foreach (var entity in entities)
			{
				entity.DoctorId = doctorId;
				entity.CreatedAt = DateTime.Now;
				await availabilityRepository.InsertAsync(entity);
			}

			transaction.Commit();

			return new DefaultServiceResponseDto
			{
				Success = true,
				Message = StaticNotifications.AvailabilityChanged.Message
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
