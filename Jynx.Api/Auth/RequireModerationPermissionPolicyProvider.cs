using Jynx.Abstractions.Entities.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Jynx.Api.Auth
{
    internal class RequireModerationPermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private const string _policyPrefix = "RequireModerationPermission";

        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

        public RequireModerationPermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
            => _fallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
            => _fallbackPolicyProvider.GetDefaultPolicyAsync()!;

        public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(_policyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                var permissionName = policyName[_policyPrefix.Length..];

                if (Enum.TryParse<ModerationPermission>(permissionName, out var permission))
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .AddRequirements(new RequireModerationPermissionRequirement(permission));

                    return policy.Build();
                }
            }

            return await _fallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
