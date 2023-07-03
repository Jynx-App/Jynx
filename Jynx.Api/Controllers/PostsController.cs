using Jynx.Api.Models.Requests;
using Jynx.Api.Models.Responses;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Controllers
{
    public class PostsController : BaseController
    {
        private const string _notFoundMessage = "Post not found";
        private const string _notAllowedToPostMessage = "You are not allowed to post to this district";

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

        [HttpPost()]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Create(CreatePostRequest request)
        {
            if (!ModelState.IsValid)
                return ModelStateError();

            var userId = Request.HttpContext.User.GetId()!;

            if (!await _districtsService.IsUserAllowedToPostAsync(request.DistrictId, userId))
                return BadRequest(_notAllowedToPostMessage);

            var post = request.ToEntity();

            var id = await _postsService.CreateAsync(post);

            return Ok(id);
        }

        [HttpGet()]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Read(string id)
        {
            var post = await _postsService.ReadAsync(id);

            if (post is null)
                return NotFound(_notFoundMessage);

            var response = new ReadPostResponse(post);

            return Ok(response);
        }

        [HttpPost()]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Update(UpdatePostRequest request)
        {
            if (!ModelState.IsValid)
                return ModelStateError();

            var userId = Request.HttpContext.User.GetId()!;

            var post = await _postsService.ReadAsync(request.Id);

            if (post is null || post.UserId != userId)
                return NotFound(_notFoundMessage);

            if (!await _districtsService.IsUserAllowedToPostAsync(post.DistrictId, userId))
                return BadRequest(_notAllowedToPostMessage);

            request.PatchEntity(post);

            post.EditedById = userId;

            await _postsService.UpdateAsync(post);

            return Ok();
        }

        [HttpPost()]
        [ApiVersion("1.0")]
        public async Task<IActionResult> Remove(string id)
        {
            var userId = Request.HttpContext.User.GetId()!;

            var post = await _postsService.ReadAsync(id);

            if (post is null || post.UserId != userId)
                return NotFound(_notFoundMessage);

            await _postsService.RemoveAsync(post);

            return Ok();
        }
    }
}
