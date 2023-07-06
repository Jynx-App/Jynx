using Jynx.Api.Models.Requests;
using Jynx.Api.Models.Responses;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Jynx.Common.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class CommentsController : RepositoryServiceController<ICommentsService, Comment>
    {
        private const string _postNotFoundMessage = "Post not found";
        private const string _notAllowedToCommentMessage = "You are not allowed to comment in this district";
        private const string _lockedMessage = "Post is locked, no new comments can be created";

        private readonly IDistrictsService _districtsService;
        private readonly IPostsService _postsService;

        public CommentsController(
            IDistrictsService districtsService,
            IPostsService postsService,
            ICommentsService commentsService,
            ILogger<CommentsController> logger)
            : base(commentsService, logger)
        {
            _districtsService = districtsService;
            _postsService = postsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest request)
        {
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

            var id = await RepositoryService.CreateAsync(entity);

            return Ok($"\"{id}\"");
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Read(string id)
        {
            var entity = await RepositoryService.GetAsync(id);

            if (entity is null)
                return NotFound(DefaultNotFoundMessage);

            var response = new ReadCommentResponse(entity);

            return Ok(response);
        }

        [HttpGet("{postId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByPostId(string postId)
        {
            var entities = await RepositoryService.GetByPostIdAsync(postId);

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

            var entity = await RepositoryService.GetAsync(request.Id);

            if (entity is null || entity.UserId != userId)
                return NotFound(DefaultNotFoundMessage);

            if (!await _districtsService.IsUserAllowedToPostAndCommentAsync(entity.DistrictId, userId))
                return BadRequest(_notAllowedToCommentMessage);

            RepositoryService.Patch(entity, request);

            entity.EditedById = userId;

            await RepositoryService.UpdateAsync(entity);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] IdRequest? request)
        {
            if (request is null || !ModelState.IsValid)
                return ModelStateError(request);

            var userId = Request.HttpContext.User.GetId()!;

            var entity = await RepositoryService.GetAsync(request.Id);

            if (entity is null || entity.UserId != userId)
                return NotFound(DefaultNotFoundMessage);

            await RepositoryService.RemoveAsync(request.Id);

            return Ok();
        }
    }
}
