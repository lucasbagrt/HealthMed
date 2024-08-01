using Availability.Domain.Dtos;
using Availability.Domain.Interfaces.Services;
using HealthMed.CrossCutting.Notifications;
using HealthMed.Domain.Dtos.Default;
using HealthMed.Domain.Extensions;
using HealthMed.Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace Availability.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AvailabilityController(IAvailabilityService availabilityService) : Controller
	{
		private readonly IAvailabilityService _availabilityService = availabilityService;

		[HttpPost]
		[Authorize(Roles = StaticUserRoles.DOCTOR)]
		[SwaggerOperation(Summary = "Add availability")]
		[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
		[SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError)]
		[SwaggerResponse((int)HttpStatusCode.Unauthorized)]
		public async Task<IActionResult> Add([FromBody] AddAvailabilityDto addAvailabilityDto)
		{
			var addResult = await _availabilityService.AddAvailabilityAsync(addAvailabilityDto, this.GetUserIdLogged());
			return Ok(addResult);
		}

		[HttpPut]
		[Authorize(Roles = StaticUserRoles.DOCTOR)]
		[SwaggerOperation(Summary = "Update availability")]
		[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DefaultServiceResponseDto))]
		[SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError)]
		[SwaggerResponse((int)HttpStatusCode.Unauthorized)]
		public async Task<IActionResult> Update([FromBody] AvailabilityDto availabilityDto)
		{
			var addResult = await _availabilityService.UpdateAvailabilityAsync(availabilityDto, this.GetUserIdLogged());
			return Ok(addResult);
		}

		[HttpGet]
		[Authorize(Roles = StaticUserRoles.DOCTOR)]
		[SwaggerOperation(Summary = "Get availabilities")]
		[SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AvailabilityDto))]
		[SwaggerResponse((int)HttpStatusCode.BadRequest, Type = typeof(IReadOnlyCollection<Notification>))]
		[SwaggerResponse((int)HttpStatusCode.NotFound)]
		[SwaggerResponse((int)HttpStatusCode.InternalServerError)]
		public async Task<IActionResult> GetAvailabilities()
		{
			var result = await _availabilityService.GetAvailabilitiesByDoctorAsync(this.GetUserIdLogged());
			if (result?.Any() == false)
				return NotFound();

			return Ok(result);
		}
	}
}
