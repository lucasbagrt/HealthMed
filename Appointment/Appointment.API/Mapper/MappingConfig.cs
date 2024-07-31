using Appointment.Domain.Dtos.Appointment;
using AutoMapper;

namespace Appointment.Api.Mapper;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {						
			config.CreateMap<AppointmentDto, Domain.Entities.Appointment>().ReverseMap();
			//config.CreateMap<AddOrderDto, OrderDto>().ReverseMap();
			//config.CreateMap<OrderItemDto, OrderItem>().ReverseMap();
			//config.CreateMap<AddOrderItemDto, OrderItem>().ReverseMap();			
		});
        return mappingConfig;
    }
}
