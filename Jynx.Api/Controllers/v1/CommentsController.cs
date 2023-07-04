using Jynx.Api.Models.Requests;
using Jynx.Api.Models.Responses;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class CommentsController : BaseController
    {
        private const string _notFoundMessage = "Comment not found";
        private const string _postNotFoundMessage = "Post not found";
        private const string _notAllowedToCommentMessage = "You are not allowed to comment in this district";
        private const string _lockedMessage = "Post is locked, no new comments can be created";

        private readonly IDistrictsService _districtsService;
        private readonly IPostsService _postsService;
        private readonly ICommentsService _commentsService;

        public CommentsController(
            IDistrictsService districtsService,
            IPostsService postsService,
            ICommentsService commentsService,
            ILogger<CommentsController> logger) : base(logger)
        {
            _districtsService = districtsService;
            _postsService = postsService;
            _commentsService = commentsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest? request)
        {
            if (request is null || !ModelState.IsValid)
                return ModelStateError(request);

            var userId = Request.HttpContext.User.GetId()!;

            var parentPost = await _postsService.GetAsync(request.PostId);

            if (parentPost is null)
                return NotFound(_postNotFoundMessage);

            if (parentPost.CommentsLocked)
                return BadRequest(_lockedMessage);

            if (!await _districtsService.IsUserAllowedToPostAndCommentAsync(request.DistrictId, userId))
                return BadRequest(_notAllowedToCommentMessage);

            var entity = request.ToEntity();

            entity.UserId = userId;

            var id = await _commentsService.CreateAsync(entity);

            return Ok($"\"{id}\"");
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Read(string id)
        {
            var entity = await _commentsService.GetAsync(id);

            if (entity is null)
                return NotFound(_notFoundMessage);

            var response = new ReadCommentResponse(entity);

            return Ok(response);
        }

        [HttpGet("{postId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByPostId(string postId)
        {
            var entities = await _commentsService.GetByPostIdAsync(postId);

            var response = new GetCommentsByPostIdResponse
            {
                Comments = entities.Select(e => new ReadCommentResponse(e))
            };

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCommentRequest? request)
        {
            if (request is null || !ModelState.IsValid)
                return ModelStateError(request);

            var userId = Request.HttpContext.User.GetId()!;

            var entity = await _commentsService.GetAsync(request.Id);

            if (entity is null || entity.UserId != userId)
                return NotFound(_notFoundMessage);

            if (!await _districtsService.IsUserAllowedToPostAndCommentAsync(entity.DistrictId, userId))
                return BadRequest(_notAllowedToCommentMessage);

            _commentsService.Patch(entity, request);

            entity.EditedById = userId;

            await _commentsService.UpdateAsync(entity);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] IdRequest? request)
        {
            if (request is null || !ModelState.IsValid)
                return ModelStateError(request);

            var userId = Request.HttpContext.User.GetId()!;

            var entity = await _commentsService.GetAsync(request.Id);

            if (entity is null || entity.UserId != userId)
                return NotFound(_notFoundMessage);

            await _commentsService.RemoveAsync(request.Id);

            return Ok();
        }
    }
}
