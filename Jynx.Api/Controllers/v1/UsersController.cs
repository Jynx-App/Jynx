using Jynx.Api.Models.Requests;
using Jynx.Api.Models.Responses;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class UsersController : BaseController
    {
        private const string _notFoundMessage = "User not found";

        private readonly IUsersService _usersService;

        public UsersController(
            IUsersService usersService,
            ILogger<UsersController> logger)
            : base(logger)
        {
            _usersService = usersService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            if (!ModelState.IsValid)
                return ModelStateError();

            var entity = request.ToEntity();

            _ = await _usersService.CreateAsync(entity);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Read(string id)
        {
            var entity = await _usersService.ReadAsync(id);

            if (entity is null)
                return NotFound(_notFoundMessage);

            var response = new ReadUserResponse(entity);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                return ModelStateError();

            var userId = Request.HttpContext.User.GetId()!;

            var entity = await _usersService.ReadAsync(request.Id);

            if (entity is null || entity.Id != userId)
                return NotFound(_notFoundMessage);

            _usersService.Patch(entity, request);

            await _usersService.UpdateAsync(entity);

            return Ok();
        }
    }
}
