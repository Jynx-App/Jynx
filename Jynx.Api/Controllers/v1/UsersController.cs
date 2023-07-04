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
    public class UsersController : BaseController
    {
        private const string _notFoundMessage = "User not found";

        private readonly IUsersService _usersService;
        private readonly IApiAppUsersService _apiAppUsersService;
        private readonly IOptions<OfficalApiAppOptions> _officalApiAppOptions;

        public UsersController(
            IUsersService usersService,
            IApiAppUsersService apiAppUsersService,
            IOptions<OfficalApiAppOptions> officalApiAppOptions,
            ILogger<UsersController> logger)
            : base(logger)
        {
            _usersService = usersService;
            _apiAppUsersService = apiAppUsersService;
            _officalApiAppOptions = officalApiAppOptions;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (request is null || !ModelState.IsValid)
                return ModelStateError(request);

            var user = request.ToEntity();

            user.Id = await _usersService.CreateAsync(user);

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
            var entity = await _usersService.ReadByUsernameAsync(username);

            if (entity is null)
                return NotFound(_notFoundMessage);

            var response = new ReadUserResponse(entity);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                return ModelStateError();

            var userId = Request.HttpContext.User.GetId()!;

            var entity = await _usersService.ReadAsync(userId);

            if (entity is null)
                return NotFound(_notFoundMessage);

            _usersService.Patch(entity, request);

            await _usersService.UpdateAsync(entity);

            return Ok();
        }
    }
}
