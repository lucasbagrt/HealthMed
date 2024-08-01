using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Appointment.Infra.Data.Mapping
{
    public class AppointmentMap : IEntityTypeConfiguration<Domain.Entities.Appointment>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Appointment> builder)
        {
            builder.ToTable("Appointment");
            builder.HasKey(a => a.Id);

            builder.Property(a => a.DoctorId)
                             .IsRequired()
                             .HasColumnName("DoctorId")
                             .HasColumnType("int");

            builder.Property(a => a.PatientId)
                   .IsRequired()
                   .HasColumnName("PatientId")
                   .HasColumnType("int");

            builder.Property(a => a.Date)
                   .IsRequired()
                   .HasColumnName("Date")
                   .HasColumnType("date");

            builder.Property(a => a.Time)
                   .IsRequired()
                   .HasColumnName("Time")
                   .HasColumnType("time");

            builder.Property(a => a.Status)
                   .IsRequired()
                   .HasColumnName("Status")
                   .HasColumnType("varchar(50)");

            builder.Property(a => a.IsActive)
                   .HasColumnName("IsActive")
                   .HasColumnType("bit")
                   .IsRequired();

            builder.Property(a => a.CreatedAt)
                   .HasColumnName("CreatedAt")
                   .HasColumnType("datetime")
                   .HasDefaultValueSql("GETDATE()")
                   .IsRequired();

        }
    }
}
