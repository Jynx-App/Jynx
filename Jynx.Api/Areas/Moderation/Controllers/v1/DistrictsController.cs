using Jynx.Abstractions.Entities.Auth;
using Jynx.Abstractions.Services;
using Jynx.Api.Areas.Moderation.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Areas.Moderation.Controllers.v1
{
    [ApiVersion("1.0")]
    public class DistrictsController : ModerationBaseController
    {
        private readonly IDistrictsService _districtsService;

        public DistrictsController(
            IDistrictsService districtsService,
            ILogger<DistrictsController> logger)
            : base(districtsService, logger)
        {
            _districtsService = districtsService;
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDistrictRequest request)
        {
            var district = await _districtsService.GetAsync(request.Id);

            if (district is null)
                return NotFound();

            var hasPermission = await DoesCurrentUserHavePermissionAsync(request.Id, ModerationPermission.EditDistrict);

            if (!hasPermission)
                return Unauthorized();

            _districtsService.Patch(district, request);

            await _districtsService.UpdateAsync(district);

            return Ok();
        }
    }
}
