using Jynx.Abstractions.Entities.Auth;
using Jynx.Abstractions.Services;
using Jynx.Api.Security.Claims;

namespace Jynx.Api.Controllers
{
    public class DistrictRelatedController : BaseController
    {

        public DistrictRelatedController(
            IDistrictsService districtsService,
            ILogger logger)
            : base(logger)
        {
            DistrictsService = districtsService;
        }

        public IDistrictsService DistrictsService { get; }

        protected async Task<bool> DoesCurrentUserHavePermissionAsync(string districtId, ModerationPermission permission)
        {
            var userId = User.GetId()!;

            var hasPermission = await DistrictsService.DoesUserHavePermissionAsync(districtId, userId, ModerationPermission.PinPosts);

            return hasPermission;
        }
    }
}
