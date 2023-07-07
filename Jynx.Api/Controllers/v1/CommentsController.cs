﻿using Jynx.Abstractions.Services;
using Jynx.Api.Models.Requests;
using Jynx.Api.Models.Responses;
using Jynx.Common.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class CommentsController : BaseController
    {
        private readonly IDistrictsService _districtsService;
        private readonly IPostsService _postsService;
        private readonly ICommentsService _commentsService;

        public CommentsController(
            IDistrictsService districtsService,
            IPostsService postsService,
            ICommentsService commentsService,
            ILogger<CommentsController> logger)
            : base(logger)
        {
            _districtsService = districtsService;
            _postsService = postsService;
            _commentsService = commentsService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest request)
        {
            var userId = Request.HttpContext.User.GetId()!;

            var parentPost = await _postsService.GetAsync(request.PostId);

            if (parentPost is null)
                return NotFound(_postsService.DefaultNotFoundMessage);

            if (parentPost.CommentsLocked)
                return BadRequest(_postsService.DefaultLockedMessage);

            if (!await _districtsService.IsUserAllowedToPostAndCommentAsync(request.DistrictId, userId))
                return BadRequest(_districtsService.DefaultNotAllowedToCommentMessage);

            var entity = request.ToEntity();

            entity.UserId = userId;

            var id = await _commentsService.CreateAsync(entity);

            return Ok($"\"{id}\"");
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string id)
        {
            var entity = await _commentsService.GetAsync(id);

            if (entity is null)
                return NotFound(_commentsService.DefaultNotFoundMessage);

            var response = new GetCommentResponse(entity);

            return Ok(response);
        }

        [HttpGet("{postId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByPostId(string postId)
        {
            var entities = await _commentsService.GetByPostIdAsync(postId);

            var response = new GetCommentsByPostIdResponse
            {
                Comments = entities.Select(e => new GetCommentResponse(e))
            };

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCommentRequest request)
        {
            var userId = Request.HttpContext.User.GetId()!;

            var entity = await _commentsService.GetAsync(request.Id);

            if (entity is null || entity.UserId != userId)
                return NotFound(_commentsService.DefaultNotFoundMessage);

            if (!await _districtsService.IsUserAllowedToPostAndCommentAsync(entity.DistrictId, userId))
                return BadRequest(_districtsService.DefaultNotAllowedToCommentMessage);

            _commentsService.Patch(entity, request);

            entity.EditedById = userId;

            await _commentsService.UpdateAsync(entity);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Remove([FromBody] IdRequest request)
        {
            var userId = Request.HttpContext.User.GetId()!;

            var entity = await _commentsService.GetAsync(request.Id);

            if (entity is null || entity.UserId != userId)
                return NotFound(_commentsService);

            await _commentsService.RemoveAsync(request.Id);

            return Ok();
        }
    }
}
