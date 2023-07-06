using Jynx.Abstractions.Entities.Auth;
using Jynx.Abstractions.Services;
using Jynx.Api.Areas.Moderation.Models.Requests;
using Jynx.Common.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Areas.Moderation.Controllers.v1
{
    [ApiVersion("1.0")]
    public class DistrictsController : ModerationBaseController
    {
        private const string _notFoundMessage = "District not found";

        private readonly IDistrictsService _districtsService;

        public DistrictsController(
            IDistrictsService districtsService,
            ILogger<DistrictsController> logger)
            : base(logger)
        {
            _districtsService = districtsService;
        }

        [HttpPut]
        [RequireModerationPermission(ModerationPermission.EditDistrict)]
        public async Task<IActionResult> Update([FromBody] UpdateDistrictRequest request)
        {
            var entity = await _districtsService.GetAsync(request.Id);

            if (entity is null)
                return NotFound(_notFoundMessage);

            _districtsService.Patch(entity, request);

            await _districtsService.UpdateAsync(entity);

            return Ok();
        }
    }
}
