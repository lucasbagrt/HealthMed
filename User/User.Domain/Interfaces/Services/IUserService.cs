using HealthMed.Domain.Dtos.Default;
using User.Domain.Dtos.User;
using User.Domain.Filters;
using Microsoft.AspNetCore.Http;
using HealthMed.Domain.Dtos;

namespace User.Domain.Interfaces.Services;

public interface IUserService
{
    Task<ICollection<UserResponseDto>> GetAllAsync(UserFilter filter);
    Task<UserResponseDto> GetByIdAsync(int id);
    Task<UserInfoDto> GetUserInfoAsync(int id, int userIdLogged);
    Task<DefaultServiceResponseDto> UpdateAsync(UpdateUserDto updateUserDto, int id);
    Task<DefaultServiceResponseDto> UpdatePasswordAsync(UpdateUserPasswordDto updateUserPasswordDto, int id);
    Task<DefaultServiceResponseDto> DeleteAsync(int id);
    Task<DefaultServiceResponseDto> ActivateAsync(ActivateUserDto activateUserDto);
}
