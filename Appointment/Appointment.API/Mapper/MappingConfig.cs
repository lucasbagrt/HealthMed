using Appointment.Domain.Dtos.Appointment;
using Appointment.Domain.Dtos.Availability;
using Appointment.Domain.Entities;
using AutoMapper;

namespace Appointment.Api.Mapper;

public static class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {						
			config.CreateMap<AppointmentDto, Domain.Entities.Appointment>().ReverseMap();
            config.CreateMap<AvailabilityDto, Availability>().ReverseMap();
            config.CreateMap<CreateAvailabilityDto, Availability>().ReverseMap();
            config.CreateMap<UpdateAvailabilityDto, Availability>().ReverseMap();
            config.CreateMap<CreateAppointmentRequestDto, Domain.Entities.Appointment>().ReverseMap();
            config.CreateMap<UpdateAppointmentRequestDto, Domain.Entities.Appointment>().ReverseMap();
        });
        return mappingConfig;
    }
}
