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

namespace Appointment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        [Authorize(Roles = StaticUserRoles.USER)]
        [SwaggerOperation(Summary = "Create an appointment")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentRequestDto request)
        {
            var result = await _appointmentService.CreateAppointmentAsync(request, this.GetUserIdLogged(), this.GetAccessToken());
            if (result == null || !result.Success)
            {
                return BadRequest(result?.Message);
            }
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
            var result = await _appointmentService.UpdateAppointmentAsync(request, this.GetUserIdLogged());
            if (result == null || !result.Success)
            {
                return BadRequest(result?.Message);
            }
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
            var result = await _appointmentService.CancelAppointmentAsync(appointmentId, this.GetUserIdLogged());
            if (result == null || !result.Success)
            {
                return BadRequest(result?.Message);
            }
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
            var result = await _appointmentService.GetAppointmentByIdAsync(appointmentId);
            if (result == null)
            {
                return NotFound();
            }
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
            var result = await _appointmentService.GetAppointmentsByDoctorIdAsync(this.GetUserIdLogged());
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
            var result = await _appointmentService.GetAppointmentsByPatientIdAsync(this.GetUserIdLogged());
            return Ok(result);
        }

        [HttpGet("available-slots")]
        [Authorize(Roles = StaticUserRoles.DOCTOR)]
        [SwaggerOperation(Summary = "Get available slots for a doctor on a specific date")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(List<AvailableSlotDto>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetAvailableSlots([FromQuery] DateTime date)
        {
            var result = await _appointmentService.GetAvailableSlotsAsync(this.GetUserIdLogged(), date);
            return Ok(result);
        }
    }
}
