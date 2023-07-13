using Jynx.Abstractions.Services;
using Jynx.Api.Models.Requests;
using Jynx.Api.Models.Responses;
using Jynx.Api.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ApiAppsController : BaseController
    {
        private readonly IApiAppsService _apiAppService;

        public ApiAppsController(
            IApiAppsService apiAppService,
            ILogger<CommentsController> logger)
            : base(logger)
        {
            _apiAppService = apiAppService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateApiAppRequest request)
        {
            if (request is null)
                return NullRequestError();

            var userId = Request.HttpContext.User.GetId()!;

            var entity = request.ToEntity();

            entity.UserId = userId;

            var apiApp = await _apiAppService.CreateAsync(entity);

            var newEntityUrl = Url.ActionLink(nameof(Get), null, new { apiApp.Id })!;

            return Created(newEntityUrl, $"\"{apiApp.Id}\"");
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string id)
        {
            var entity = await _apiAppService.GetAsync(id);

            if (entity is null)
                return NotFound(_apiAppService.DefaultNotFoundMessage);

            var response = new GetApiAppResponse(entity);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateApiAppRequest request)
        {
            if (request is null)
                return NullRequestError();

            var userId = Request.HttpContext.User.GetId()!;

            var entity = await _apiAppService.GetAsync(request.Id);

            if (entity is null || entity.UserId != userId)
                return NotFound(_apiAppService.DefaultNotFoundMessage);

            _apiAppService.Patch(entity, request);

            await _apiAppService.UpdateAsync(entity);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] IdRequest request)
        {
            if (request is null)
                return NullRequestError();

            var userId = Request.HttpContext.User.GetId()!;

            var entity = await _apiAppService.GetAsync(request.Id);

            if (entity is null || entity.UserId != userId)
                return NotFound(_apiAppService);

            await _apiAppService.RemoveAsync(request.Id);

            return Ok();
        }
    }
}
