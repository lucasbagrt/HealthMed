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
		}
	}
}
