using Appointment.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointment.Infra.Data.Mapping;

public class AppointmentMap : IEntityTypeConfiguration<Domain.Entities.Appointment>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Appointment> builder)
    {
        builder.ToTable("Appointment");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.PatientId)
               .IsRequired()
               .HasColumnName("PatientId")
               .HasColumnType("int");

        builder.Property(a => a.AvailabilityId)
               .IsRequired()
               .HasColumnName("AvailabilityId")
               .HasColumnType("int");

        builder.Property(a => a.Status)
              .IsRequired()
              .HasColumnName("Status")
              .HasColumnType("varchar(50)")
              .HasConversion(
                  s => s.ToString(),
                  s => (AppointmentStatus)Enum.Parse(typeof(AppointmentStatus), s));

        builder.Property(a => a.IsActive)
               .HasColumnName("IsActive")
               .HasColumnType("bit")
               .IsRequired();

        builder.Property(a => a.CreatedAt)
               .HasColumnName("CreatedAt")
               .HasColumnType("datetime")
               .HasDefaultValueSql("GETDATE()")
               .IsRequired();

        builder.HasOne(a => a.Availability)
               .WithMany()
               .HasForeignKey(a => a.AvailabilityId);
    }
}
