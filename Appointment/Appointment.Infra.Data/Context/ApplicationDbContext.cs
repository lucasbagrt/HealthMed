using Appointment.Infra.Data.Mapping;
using Availability.Domain.Entities;
using Availability.Infra.Data.Mapping;
using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Infra.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Appointment> Appointments { get; set; }
    public DbSet<Availability.Domain.Entities.Availability> Availabilities { get; set; }
    public DbSet<SeedHistory> SeedHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AppointmentMap());
        modelBuilder.ApplyConfiguration(new AvailabilityMap());
    }
}
