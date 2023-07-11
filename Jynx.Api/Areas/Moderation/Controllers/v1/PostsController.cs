using Jynx.Abstractions.Entities.Auth;
using Jynx.Abstractions.Services;
using Jynx.Api.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Areas.Moderation.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PostsController : ModerationBaseController
    {
        private readonly IPostsService _postsService;

        public PostsController(
            IPostsService postsService,
            IDistrictsService districtsService,
            ILogger<PostsController> logger)
            : base(districtsService, logger)
        {
            _postsService = postsService;
        }

        [HttpPut]
        public async Task<IActionResult> Pin([FromBody] IdRequest request)
        {
            var post = await _postsService.GetAsync(request.Id);

            if (post is null)
                return NotFound();

            var hasPermission = await DoesCurrentUserHavePermissionAsync(post.DistrictId, ModerationPermission.PinPosts);

            if (!hasPermission)
                return Unauthorized();

            _ = await _postsService.PinAsync(post);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Unpin([FromBody] IdRequest request)
        {
            var post = await _postsService.GetAsync(request.Id);

            if (post is null)
                return NotFound();

            var hasPermission = await DoesCurrentUserHavePermissionAsync(post.DistrictId, ModerationPermission.PinPosts);

            if (!hasPermission)
                return Unauthorized();

            _ = await _postsService.UnpinAsync(post);

            return Ok();
        }
    }
}
