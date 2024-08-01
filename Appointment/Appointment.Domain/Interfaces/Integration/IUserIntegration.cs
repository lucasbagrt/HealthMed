using HealthMed.Domain.Dtos;

namespace Appointment.Domain.Interfaces.Integration;

public interface IUserIntegration 
{
    Task<UserInfoDto> GetUserInfo(int idUser, string token);
}
