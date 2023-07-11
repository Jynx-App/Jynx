using Jynx.Abstractions.Entities.Auth;
using Jynx.Abstractions.Services;
using Jynx.Api.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Areas.Moderation.Controllers.v1
{
    public class CommentsController : ModerationBaseController
    {
        private readonly ICommentsService _commentsService;

        public CommentsController(
            IDistrictsService districtsService,
            ICommentsService commentsService,
            ILogger<CommentsController> logger)
            : base(districtsService, logger)
        {
            _commentsService = commentsService;
        }

        [HttpPut]
        public async Task<IActionResult> Pin([FromBody] IdRequest request)
        {
            var comment = await _commentsService.GetAsync(request.Id);

            if (comment is null)
                return NotFound();

            var hasPermission = await DoesCurrentUserHavePermissionAsync(comment.DistrictId, ModerationPermission.PinPosts);

            if (!hasPermission)
                return Unauthorized();

            _ = await _commentsService.PinAsync(comment);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Unpin([FromBody] IdRequest request)
        {
            var comment = await _commentsService.GetAsync(request.Id);

            if (comment is null)
                return NotFound();

            var hasPermission = await DoesCurrentUserHavePermissionAsync(comment.DistrictId, ModerationPermission.PinPosts);

            if (!hasPermission)
                return Unauthorized();

            _ = await _commentsService.UnpinAsync(comment);

            return Ok();
        }
    }
}
