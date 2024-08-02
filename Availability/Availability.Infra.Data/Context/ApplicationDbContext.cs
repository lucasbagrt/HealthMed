using Microsoft.EntityFrameworkCore;
using Availability.Infra.Data.Mapping;

namespace Availability.Infra.Data.Context
{
	public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
	{
		public DbSet<Domain.Entities.Availability> Availabilities { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Domain.Entities.Availability>(new AvailabilityMap().Configure);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies();
		}
	}
}
