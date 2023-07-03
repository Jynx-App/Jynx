using Jynx.Api.Areas.Moderation.Models.Requests;
using Jynx.Common.Abstractions.Services;
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

        [HttpPost]
        [RequireModerationPermission(ModerationPermission.EditDistrict)]
        public async Task<IActionResult> Update(UpdateDistrictRequest request)
        {
            if (!ModelState.IsValid)
                return ModelStateError();

            var entity = await _districtsService.ReadAsync(request.Id);

            if (entity is null)
                return NotFound(_notFoundMessage);

            _districtsService.Patch(entity, request);

            await _districtsService.UpdateAsync(entity);

            return Ok();
        }

        [HttpGet("{id}")]
        [RequireModerationPermission(ModerationPermission.ApproveComments)]
        public async Task<IActionResult> TestEndpoint(string id)
        {
            return Ok("Test");
        }
    }
}
