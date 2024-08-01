using Appointment.Infra.Data.Mapping;
using HealthMed.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointment.Infra.Data.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Domain.Entities.Appointment> Appointments { get; set; }
    public DbSet<SeedHistory> SeedHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new AppointmentMap());
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}
