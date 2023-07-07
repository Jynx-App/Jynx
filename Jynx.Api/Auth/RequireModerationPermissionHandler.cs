using Jynx.Abstractions.Entities.Auth;
using Jynx.Abstractions.Services;
using Jynx.Api.Security.Claims;
using Jynx.Common.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Jynx.Api.Auth
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
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext is null)
                return;

            if (context.User is null)
                return;

            var districtId = httpContext.GetRouteData().Values.ContainsKey("id")
                ? httpContext.GetRouteData().Values["id"]?.ToString()
                : null;

            var httpMethod = httpContext.Request.Method.ToLower();

            if (string.IsNullOrWhiteSpace(districtId) && _httpMethodsThatUseModel.Contains(httpMethod))
            {
                var json = await httpContext.Request.GetBodyAsStringAsync();

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
