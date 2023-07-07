using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;
using Jynx.Api.Models.Requests;
using Jynx.Api.Models.Responses;
using Jynx.Common.Configuration;
using Jynx.Common.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Jynx.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class UsersController : BaseController
    {
        private readonly IUsersService _usersService;
        private readonly IApiAppUsersService _apiAppUsersService;
        private readonly IOptions<OfficialApiAppOptions> _officialApiAppOptions;

        public UsersController(
            IUsersService usersService,
            IApiAppUsersService apiAppUsersService,
            IOptions<OfficialApiAppOptions> officialApiAppOptions,
            ILogger<UsersController> logger)
            : base(logger)
        {
            _usersService = usersService;
            _apiAppUsersService = apiAppUsersService;
            _officialApiAppOptions = officialApiAppOptions;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var user = request.ToEntity();

            user.Id = await _usersService.CreateAsync(user);

            var apiAppUser = new ApiAppUser
            {
                ApiAppId = _officialApiAppOptions.Value.Id,
                UserId = user.Id
            };

            var id = await _apiAppUsersService.CreateAsync(apiAppUser);

            var username = user.Username;

            var newEntityUrl = Url.ActionLink(nameof(Get), null, new { username })!;

            return Created(newEntityUrl, $"\"{username}\"");
        }

        [HttpGet("{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string username)
        {
            var entity = await _usersService.GetByUsernameAsync(username);

            if (entity is null)
                return NotFound(_usersService.DefaultNotFoundMessage);

            var response = new GetUserResponse(entity);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            var userId = Request.HttpContext.User.GetId()!;

            var entity = await _usersService.GetAsync(userId);

            if (entity is null)
                return NotFound(_usersService.DefaultNotFoundMessage);

            _usersService.Patch(entity, request);

            await _usersService.UpdateAsync(entity);

            return Ok();
        }
    }
}
