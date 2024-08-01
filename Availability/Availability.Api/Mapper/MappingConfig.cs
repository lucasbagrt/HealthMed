using AutoMapper;
using Availability.Domain.Dtos;

namespace Availability.API.Mapper
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			var mappingConfig = new MapperConfiguration(config =>
			{
				config.CreateMap<AvailabilityDto, Domain.Entities.Availability>().ReverseMap();
			});
			return mappingConfig;
		}
	}
}
