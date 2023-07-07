using System.Security.Claims;

namespace Jynx.Api.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetId(this ClaimsPrincipal principal)
            => principal.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}
