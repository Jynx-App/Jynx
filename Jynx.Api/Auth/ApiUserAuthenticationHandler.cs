using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;
using Jynx.Common.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Jynx.Api.Auth
{
    public class ApiUserAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private const string _invalidApiKeyMessage = "Invalid Api Key";
        private const string _invalidUserMessage = "Invalid User";
        private readonly IUsersService _usersService;
        private readonly IApiAppUsersService _apiAppUsersService;

        public const string Schema = "ApiUser";

        public ApiUserAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            IUsersService usersService,
            IApiAppUsersService apiAppUsersService,
            ILoggerFactory logger, UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _usersService = usersService;
            _apiAppUsersService = apiAppUsersService;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (IsSwaggerEndpoint())
                return AuthenticateResult.NoResult();

            var endpoint = Context.Features.Get<IEndpointFeature>()?.Endpoint;

            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is not null)
                return AuthenticateResult.NoResult();

            var apiKey = Request.GetAuthenticationValue();

            if (string.IsNullOrWhiteSpace(apiKey))
                return AuthenticateResult.Fail(_invalidApiKeyMessage);

            var apiAppUser = await _apiAppUsersService.GetAsync(apiKey);

            if (apiAppUser is null)
                return AuthenticateResult.Fail(_invalidApiKeyMessage);

            var user = await _usersService.GetAsync(apiAppUser.UserId);

            if (user is null)
                return AuthenticateResult.Fail(_invalidUserMessage);

            var claims = BuildClaims(user);

            var ticket = BuildTicket(claims);

            return AuthenticateResult.Success(ticket);
        }

        private bool IsSwaggerEndpoint()
        {
            var path = Context.Request.Path.ToString();

            return path.StartsWith("/swagger/", StringComparison.OrdinalIgnoreCase);
        }

        private static IEnumerable<Claim> BuildClaims(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id!),
                new(ClaimTypes.Name, user.Username),
            };

            return claims;
        }

        private static AuthenticationTicket BuildTicket(IEnumerable<Claim> claims)
        {
            var identity = new ClaimsIdentity(claims, Schema);

            var identities = new List<ClaimsIdentity> { identity };

            var principal = new ClaimsPrincipal(identities);

            return new(principal, Schema);
        }
    }
}
