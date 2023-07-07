using Jynx.Abstractions.Services;
using Jynx.Api.Models.Requests;
using Jynx.Api.Models.Responses;
using Jynx.Api.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PostsController : BaseController
    {
        private readonly IDistrictsService _districtsService;
        private readonly IPostsService _postsService;

        public PostsController(
            IDistrictsService districtsService,
            IPostsService postsService,
            ILogger<PostsController> logger) : base(logger)
        {
            _districtsService = districtsService;
            _postsService = postsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
        {
            var userId = Request.HttpContext.User.GetId()!;

            var entity = request.ToEntity();

            entity.UserId = userId;

            var id = await _postsService.CreateAsync(entity);

            var newEntityUrl = Url.ActionLink(nameof(Get), null, new { id })!;

            return Created(newEntityUrl, $"\"{id}\"");
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string id)
        {
            var entity = await _postsService.GetAsync(id);

            if (entity is null)
                return NotFound(_postsService.DefaultNotFoundMessage);

            var response = new GetPostResponse(entity);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePostRequest request)
        {
            var userId = Request.HttpContext.User.GetId()!;

            var entity = await _postsService.GetAsync(request.Id);

            if (entity is null || entity.UserId != userId)
                return NotFound(_postsService.DefaultNotFoundMessage);

            _postsService.Patch(entity, request);

            entity.EditedById = userId;

            await _postsService.UpdateAsync(entity);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] IdRequest request)
        {
            var userId = Request.HttpContext.User.GetId()!;

            var entity = await _postsService.GetAsync(request.Id);

            if (entity is null || entity.UserId != userId)
                return NotFound(_postsService.DefaultNotFoundMessage);

            await _postsService.RemoveAsync(request.Id);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Vote([FromBody] PostVoteRequest request)
        {
            var userId = Request.HttpContext.User.GetId()!;

            _ = await _postsService.VoteAsync(request.PostId, userId, request.Negative);

            return Ok();
        }
    }
}
