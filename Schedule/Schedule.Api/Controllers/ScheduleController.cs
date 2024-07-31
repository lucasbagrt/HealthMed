using HealthMed.CrossCutting.Notifications;
using HealthMed.Domain.Dtos.Default;
using HealthMed.Domain.Extensions;
using HealthMed.Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Schedule.Domain.Dtos;
using Schedule.Domain.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Schedule.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ScheduleController(IScheduleService scheduleService) : Controller
	{
		private readonly IScheduleService _scheduleService = scheduleService;

		[HttpPost]
		[Authorize(Roles = StaticUserRoles.DOCTOR)]
		[SwaggerOperation(Summary = "Add schedule")]
		[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
		[SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError)]
		[SwaggerResponse((int)HttpStatusCode.Unauthorized)]
		public async Task<IActionResult> Add([FromBody] AddScheduleDto addScheduleDto)
		{
			var addResult = await _scheduleService.AddScheduleAsync(addScheduleDto, this.GetUserIdLogged());
			return Ok(addResult);
		}
	}
}
