using Appointment.Domain.Dtos.Appointment;
using Appointment.Domain.Interfaces.Services;
using HealthMed.CrossCutting.Notifications;
using HealthMed.Domain.Dtos.Default;
using HealthMed.Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using HealthMed.Domain.Extensions;
using System.Net;

namespace Appointment.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AppointmentController : Controller
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService) => _appointmentService = appointmentService;

    [HttpPost]
    [Authorize(Roles = StaticUserRoles.USER)]
    [SwaggerOperation(Summary = "Create an appointment")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentRequestDto request)
    {
        var result = await _appointmentService.CreateAsync(request, this.GetUserIdLogged(), this.GetAccessToken());        
        return Ok(result);
    }

    [HttpPut]
    [Authorize(Roles = StaticUserRoles.USER)]
    [SwaggerOperation(Summary = "Update an appointment")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Update([FromBody] UpdateAppointmentRequestDto request)
    {
        var result = await _appointmentService.UpdateAsync(request, this.GetUserIdLogged());
        return Ok(result);
    }

    [HttpDelete("{appointmentId}")]
    [Authorize(Roles = StaticUserRoles.USER)]
    [SwaggerOperation(Summary = "Cancel an appointment")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Cancel(int appointmentId)
    {
        var result = await _appointmentService.CancelAsync(appointmentId, this.GetUserIdLogged());        
        return Ok(result);
    }

    [HttpGet("{appointmentId}")]
    [Authorize(Roles = StaticUserRoles.USER)]
    [SwaggerOperation(Summary = "Get appointment by ID")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AppointmentDto))]
    [SwaggerResponse((int)HttpStatusCode.NotFound)]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetById(int appointmentId)
    {
        var result = await _appointmentService.GetByIdAsync(appointmentId);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = StaticUserRoles.USER)]
    [SwaggerOperation(Summary = "Get all appointments")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<AppointmentDto>))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _appointmentService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("doctor")]
    [Authorize(Roles = StaticUserRoles.DOCTOR)]
    [SwaggerOperation(Summary = "Get appointments by doctor ID")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DoctorScheduleResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetByDoctorId()
    {
        var result = await _appointmentService.GetByDoctorIdAsync(this.GetUserIdLogged());
        return Ok(result);
    }

    [HttpGet("patient")]
    [Authorize(Roles = StaticUserRoles.USER)]
    [SwaggerOperation(Summary = "Get appointments by patient ID")]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PatientAppointmentsResponseDto))]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> GetByPatientId()
    {
        var result = await _appointmentService.GetByPatientIdAsync(this.GetUserIdLogged());
        return Ok(result);
    }   
}
