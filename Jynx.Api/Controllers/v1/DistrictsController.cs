using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;
using Jynx.Api.Models;
using Jynx.Api.Models.Requests;
using Jynx.Api.Models.Responses;
using Jynx.Api.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class DistrictsController : BaseController
    {
        private readonly IDistrictsService _districtsService;

        public DistrictsController(
            IDistrictsService districtsService,
            ILogger<DistrictsController> logger)
            : base(logger)
        {
            _districtsService = districtsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDistrictRequest request)
        {
            var userId = Request.HttpContext.User.GetId()!;

            var district = request.ToEntity();

            district = await _districtsService.CreateAndAssignModerator(district, userId);

            var newEntityUrl = Url.ActionLink(nameof(Get), null, new { district.Id })!;

            return Created(newEntityUrl, $"\"{district.Id}\"");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var entity = await _districtsService.GetAsync(id);

            if (entity is null)
                return NotFound(_districtsService.DefaultNotFoundMessage);

            var response = new GetDistrictResponse(entity);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPosts(string id, [FromQuery] int offset = 0, [FromQuery] int count = 1000, PostsSortOrder? sortOrder = null)
        {
            var posts = await _districtsService.GetPostsAsync(id, count, offset, sortOrder);

            var postModels = posts.Select(p => new PostModel(p));

            return Ok(postModels);
        }
    }
}
