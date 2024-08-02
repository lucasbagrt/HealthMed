using Appointment.Domain.Interfaces.Integration;
using HealthMed.Domain.Dtos;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Appointment.Service.Integration;
public class UserIntegration : IUserIntegration
{
    private readonly IConfiguration _configuration;

    public UserIntegration(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<UserInfoDto> GetUserInfo(int idUser, string token)
    {
        try
        {
            using var httpClient = new HttpClient();

            var baseUrl = _configuration["Integration:User:UrlBase"];
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/api/User/{idUser}");

            return await response.Content.ReadFromJsonAsync<UserInfoDto>();
        }
        catch (Exception e)
        {

            throw;
        }
    }
}
