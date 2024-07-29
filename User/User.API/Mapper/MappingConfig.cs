using AutoMapper;
using User.Domain.Dtos.Auth;
using User.Domain.Dtos.User;
using Entities = User.Domain.Entities;

namespace User.API.Mapper;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            #region Auth
            config.CreateMap<RegisterDto, Entities.User>().ReverseMap();
            #endregion

            #region User
            config.CreateMap<UserResponseDto, Entities.User>().ReverseMap();
            config.CreateMap<UpdateUserDto, Entities.User>().ReverseMap();
            #endregion
        });
        return mappingConfig;
    }
}