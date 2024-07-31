using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Schedule.Infra.Data.Mapping
{
	public class ScheduleMap : IEntityTypeConfiguration<Domain.Entities.Schedule>
	{
		public void Configure(EntityTypeBuilder<Domain.Entities.Schedule> builder)
		{
			builder.ToTable("Schedule");
			builder.HasKey(prop => prop.Id);

			builder.HasMany(e => e.AvailableTimes)
					.WithOne(e => e.Schedule)
					.HasForeignKey(e => e.ScheduleId)
					.HasPrincipalKey(e => e.Id);
		}
	}
}
