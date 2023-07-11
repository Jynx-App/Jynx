using Jynx.Abstractions.Entities.Auth;
using Jynx.Abstractions.Services;
using Jynx.Api.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Areas.Moderation.Controllers
{
    [Area("Moderation")]
    public class ModerationBaseController : BaseAreaController
    {
        private readonly IDistrictsService _districtsService;

        public ModerationBaseController(
            IDistrictsService districtsService,
            ILogger logger)
            : base(logger)
        {
            _districtsService = districtsService;
        }

        protected async Task<bool> DoesCurrentUserHavePermissionAsync(string districtId, ModerationPermission permission)
        {
            var userId = User.GetId()!;

            var hasPermission = await _districtsService.DoesUserHavePermissionAsync(districtId, userId, ModerationPermission.PinPosts);

            return hasPermission;
        }
    }
}
