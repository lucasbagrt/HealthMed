using Microsoft.EntityFrameworkCore;
using Schedule.Infra.Data.Mapping;

namespace Schedule.Infra.Data.Context
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
	{
		public DbSet<Domain.Entities.Schedule> Schedules { get; set; }
		public DbSet<Domain.Entities.AvailableTime> AvailableTimes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Domain.Entities.Schedule>(new ScheduleMap().Configure);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies();
		}
	}
}
