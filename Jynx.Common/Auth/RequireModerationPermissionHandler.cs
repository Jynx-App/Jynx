using Jynx.Common.Abstractions.Services;
using Jynx.Common.AspNetCore.Http;
using Jynx.Common.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace Jynx.Common.Auth
{
    public class RequireModerationPermissionHandler : AuthorizationHandler<RequireModerationPermissionRequirement>
    {
        private readonly IDistrictsService _districtsService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string[] _httpMethodsThatUseModel = new[]
        {
            "post",
            "put",
            "delete",
        };

        public RequireModerationPermissionHandler(
            IDistrictsService districtsService,
            IHttpContextAccessor httpContextAccessor)
        {
            _districtsService = districtsService;
            _httpContextAccessor = httpContextAccessor;
        }

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, RequireModerationPermissionRequirement requirement)
        {
            if (context.User is null)
                return;

            var districtId = _httpContextAccessor.HttpContext.GetRouteData().Values.ContainsKey("id")
                ? _httpContextAccessor.HttpContext.GetRouteData().Values["id"].ToString()
                : null;

            var httpMethod = _httpContextAccessor.HttpContext.Request.Method.ToLower();

            if (string.IsNullOrWhiteSpace(districtId) && _httpMethodsThatUseModel.Contains(httpMethod))
            {
                var json = await _httpContextAccessor.HttpContext.Request.GetBodyAsStringAsync();

                var model = JsonConvert.DeserializeObject<RequireModerationPermissionModel>(json);

                districtId = model?.DistrictId ?? model?.Id;
            }

            var userId = context.User.GetId()!;

            if (string.IsNullOrWhiteSpace(districtId))
                return;

            var hasPermission = await _districtsService.DoesUserHavePermissionAsync(districtId, userId, requirement.Permission);

            if (hasPermission)
                context.Succeed(requirement);

            return;
        }
    }

    public class RequireModerationPermissionRequirement : IAuthorizationRequirement
    {
        public RequireModerationPermissionRequirement(ModerationPermission requiredPermission)
        {
            Permission = requiredPermission;
        }

        public ModerationPermission Permission { get; }
    }
}
