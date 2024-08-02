using AutoMapper;
using Availability.Domain.Dtos;
using Availability.Domain.Interfaces.Repositories;
using Availability.Infra.Data.Context;
using Availability.Service;
using HealthMed.CrossCutting.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Data;

namespace Availability.UnitTests.Fixtures
{
	public class AvailabilityServiceFixture
	{
		public AvailabilityService AvailabilityService { get; }

		public AvailabilityServiceFixture()
		{
			var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			optionsBuilder.UseInMemoryDatabase("test.db");
			var dbContext = new ApplicationDbContext(optionsBuilder.Options);

			var availabilityRepositoryMock = new Mock<IAvailabilityRepository>();
			var mapperMock = new Mock<IMapper>();
			var notificationContextMock = new Mock<NotificationContext>();
			var configurationMock = new Mock<IConfiguration>();

			// Cenário: GetAllAsync com filtro válido
			var users = new List<Domain.Entities.Availability>
			{
				new() { Id = 1, DoctorId = 1, Start = new(2024, 8, 1, 7, 0, 0), End = new(2024, 8, 1, 14, 30, 0) },
				new() { Id = 1, DoctorId = 1, Start = new(2024, 8, 2, 11, 30, 0), End = new(2024, 8, 2, 19, 0, 0) },
				new() { Id = 1, DoctorId = 1, Start = new(2024, 8, 3, 7, 30, 0), End = new(2024, 8, 3, 14, 30, 0) },
				new() { Id = 1, DoctorId = 1, Start = new(2024, 8, 4, 11, 30, 0), End = new(2024, 8, 4, 19, 0, 0) },

				new() { Id = 1, DoctorId = 2, Start = new(2024, 8, 1, 5, 0, 0), End = new(2024, 8, 1, 12, 30, 0) },
				new() { Id = 1, DoctorId = 2, Start = new(2024, 8, 2, 9, 30, 0), End = new(2024, 8, 2, 17, 0, 0) },
				new() { Id = 1, DoctorId = 2, Start = new(2024, 8, 3, 15, 30, 0), End = new(2024, 8, 3, 22, 30, 0) },
				new() { Id = 1, DoctorId = 2, Start = new(2024, 8, 4, 15, 0, 0), End = new(2024, 8, 4, 22, 0, 0) },
			};

			availabilityRepositoryMock.Setup(x => x.SelectAsync()).ReturnsAsync(users);

			mapperMock.Setup(x => x.Map<List<AvailabilityDto>>(It.IsAny<IEnumerable<Domain.Entities.Availability>>()))
				.Returns((IEnumerable<Domain.Entities.Availability> availabilities) => availabilities.Select(x => new AvailabilityDto { Start = x.Start, End = x.End }).ToList());

			AvailabilityService = new AvailabilityService(
				dbContext,
				availabilityRepositoryMock.Object,
				mapperMock.Object,
				notificationContextMock.Object);
		}
	}
}
