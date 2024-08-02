﻿using Appointment.Domain.Dtos.Appointment;
using AutoMapper;

namespace Appointment.Api.Mapper;

public static class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {						
			config.CreateMap<AppointmentDto, Domain.Entities.Appointment>().ReverseMap();
			config.CreateMap<CreateAppointmentRequestDto, Domain.Entities.Appointment>().ReverseMap();
			config.CreateMap<UpdateAppointmentRequestDto, Domain.Entities.Appointment>().ReverseMap();
		});
        return mappingConfig;
    }
}
