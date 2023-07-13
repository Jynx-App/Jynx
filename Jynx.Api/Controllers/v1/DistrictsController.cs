using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Entities.Auth;
using Jynx.Abstractions.Services;
using Jynx.Api.Auth;
using Jynx.Api.Models;
using Jynx.Api.Models.Requests;
using Jynx.Api.Models.Responses;
using Jynx.Api.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class DistrictsController : DistrictRelatedController
    {
        public DistrictsController(
            IDistrictsService districtsService,
            ILogger<DistrictsController> logger)
            : base(districtsService, logger)
        {

        }

        #region General Access
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDistrictRequest request)
        {
            var userId = Request.HttpContext.User.GetId()!;

            var district = request.ToEntity();

            district = await DistrictsService.CreateAndAssignModerator(district, userId);

            var newEntityUrl = Url.ActionLink(nameof(Get), null, new { district.Id })!;

            return Created(newEntityUrl, $"\"{district.Id}\"");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var entity = await DistrictsService.GetAsync(id);

            if (entity is null)
                return NotFound(DistrictsService.DefaultNotFoundMessage);

            var response = new GetDistrictResponse(entity);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPosts(string id, [FromQuery] int offset = 0, [FromQuery] int count = 1000, PostsSortOrder? sortOrder = null)
        {
            var posts = await DistrictsService.GetPostsAsync(id, count, offset, sortOrder);

            var postModels = posts.Select(p => new PostModel(p));

            return Ok(postModels);
        }
        #endregion

        #region Moderation
        [HttpPut]
        [RequireModerationPermission(ModerationPermission.EditDistrict)]
        public async Task<IActionResult> Update([FromBody] UpdateDistrictRequest request)
        {
            var district = await DistrictsService.GetAsync(request.Id);

            if (district is null)
                return NotFound();

            DistrictsService.Patch(district, request);

            await DistrictsService.UpdateAsync(district);

            return Ok();
        }
        #endregion
    }
}
