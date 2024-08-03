using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Appointment.Domain.Entities;

namespace Appointment.Infra.Data.Mapping;

public class AvailabilityMap : IEntityTypeConfiguration<Availability>
{
    public void Configure(EntityTypeBuilder<Availability> builder)
    {
        builder.ToTable("Availability");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.DoctorId)
                         .IsRequired()
                         .HasColumnName("DoctorId")
                         .HasColumnType("int");        

        builder.Property(a => a.Date)
               .IsRequired()
               .HasColumnName("Date")
               .HasColumnType("date");

        builder.Property(a => a.Time)
               .IsRequired()
               .HasColumnName("Time")
               .HasColumnType("time");
    }
}
