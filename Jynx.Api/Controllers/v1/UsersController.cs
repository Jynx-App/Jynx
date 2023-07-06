using Jynx.Api.Models.Requests;
using Jynx.Api.Models.Responses;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Configuration;
using Jynx.Common.Entities;
using Jynx.Common.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Jynx.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class UsersController : RepositoryServiceController<IUsersService, User>
    {
        private readonly IApiAppUsersService _apiAppUsersService;
        private readonly IOptions<OfficalApiAppOptions> _officalApiAppOptions;

        public UsersController(
            IUsersService usersService,
            IApiAppUsersService apiAppUsersService,
            IOptions<OfficalApiAppOptions> officalApiAppOptions,
            ILogger<UsersController> logger)
            : base(usersService, logger)
        {
            _apiAppUsersService = apiAppUsersService;
            _officalApiAppOptions = officalApiAppOptions;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest? request)
        {
            if (request is null || !ModelState.IsValid)
                return ModelStateError(request);

            var user = request.ToEntity();

            user.Id = await RepositoryService.CreateAsync(user);

            var apiAppUser = new ApiAppUser
            {
                ApiAppId = _officalApiAppOptions.Value.Id,
                UserId = user.Id
            };

            apiAppUser.Id = await _apiAppUsersService.CreateAsync(apiAppUser);

            return Ok($"\"{user.Username}\"");
        }

        [HttpGet("{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> Read(string username)
        {
            var entity = await RepositoryService.GetByUsernameAsync(username);

            if (entity is null)
                return NotFound(DefaultNotFoundMessage);

            var response = new ReadUserResponse(entity);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest? request)
        {
            if (request is null || !ModelState.IsValid)
                return ModelStateError(request);

            var userId = Request.HttpContext.User.GetId()!;

            var entity = await RepositoryService.GetAsync(userId);

            if (entity is null)
                return NotFound(DefaultNotFoundMessage);

            RepositoryService.Patch(entity, request);

            await RepositoryService.UpdateAsync(entity);

            return Ok();
        }
    }
}
