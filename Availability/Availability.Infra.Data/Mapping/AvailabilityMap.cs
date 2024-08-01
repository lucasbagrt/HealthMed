using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Availability.Infra.Data.Mapping
{
	public class AvailabilityMap : IEntityTypeConfiguration<Domain.Entities.Availability>
	{
		public void Configure(EntityTypeBuilder<Domain.Entities.Availability> builder)
		{
			builder.ToTable("Availability");
			builder.HasKey(prop => prop.Id);

			builder.HasMany(e => e.AvailableTimes)
					.WithOne(e => e.Availability)
					.HasForeignKey(e => e.AvailabilityId)
					.HasPrincipalKey(e => e.Id);
		}
	}
}
