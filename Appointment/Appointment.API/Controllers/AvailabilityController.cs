using Microsoft.AspNetCore.Mvc;
using HealthMed.Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using HealthMed.Domain.Dtos.Default;
using System.Net;
using Appointment.Domain.Interfaces.Services;
using HealthMed.CrossCutting.Notifications;
using Appointment.Domain.Dtos.Availability;
using HealthMed.Domain.Extensions;

namespace Appointment.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AvailabilityController : Controller
{
    private readonly IAvailabilityService _availabilityService;

    public AvailabilityController(IAvailabilityService vailabilityService) => _availabilityService = vailabilityService;

    [HttpPost]
    [Authorize(Roles = StaticUserRoles.DOCTOR)]
    [SwaggerOperation(Summary = "Create availability")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateAvailabilityDto createAvailabilityDto)
    {
        var addResult = await _availabilityService.CreateAsync(createAvailabilityDto, this.GetUserIdLogged());
        return Ok(addResult);
    }

    [HttpPut]
    [Authorize(Roles = StaticUserRoles.DOCTOR)]
    [SwaggerOperation(Summary = "Update availability")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateAvailabilityDto updateAvailabilityDto)
    {
        var result = await _availabilityService.UpdateAsync(updateAvailabilityDto, this.GetUserIdLogged());
        return Ok(result);
    }

    [HttpGet("{doctorId}")]
    [SwaggerOperation(Summary = "Get availabilities")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AvailabilityDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> GetAvailabilitiesByDoctorAsync(int doctorId)
    {
        var result = await _availabilityService.GetByDoctorAsync(doctorId);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = StaticUserRoles.DOCTOR)]
    [SwaggerOperation(Summary = "Delete an availability")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var result = await _availabilityService.DeleteAsync(id, this.GetUserIdLogged());
        return Ok(result);
    }
}
