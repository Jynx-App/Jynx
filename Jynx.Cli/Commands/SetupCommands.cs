using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;
using Jynx.Common.Services.Exceptions;

namespace Jynx.Cli.Commands
{
    [Command("setup")]
    internal class SetupCommands : ConsoleAppBase
    {
        private readonly IUsersService _usersService;
        private readonly IApiAppsService _apiAppService;
        private readonly IApiAppUsersService _apiAppUsersService;

        public SetupCommands(
            IUsersService usersService,
            IApiAppsService apiAppService,
            IApiAppUsersService apiAppUsersService
            )
        {
            _usersService = usersService;
            _apiAppService = apiAppService;
            _apiAppUsersService = apiAppUsersService;
        }

        public async Task QuickStart(string username, string password, string email)
        {
            try
            {
                var user = new User
                {
                    Username = username,
                    Password = password,
                    Email = email
                };

                user.Id = await _usersService.CreateAsync(user);

                var apiApp = new ApiApp
                {
                    Name = "Official Web App",
                    UserId = user.Id,
                    CallbackUrl = "https://google.com" // this needs to be changed when frontend feature is implemented.
                };

                apiApp.Id = await _apiAppService.CreateAsync(apiApp);

                var apiAppUser = new ApiAppUser
                {
                    ApiAppId = apiApp.Id,
                    UserId = user.Id,
                };

                apiAppUser.Id = await _apiAppUsersService.CreateAsync(apiAppUser);

                Console.WriteLine("Setup complete! You're ready to start using the API!");
                Console.WriteLine($"User: {user.Id}");
                Console.WriteLine($"ApiApp: {apiApp.Id}");
                Console.WriteLine($"ApiAppUser: {apiAppUser.Id} <-- this is the \"key\" you use in your authentication header when making HTTP calls to Jynx.Api");
            }
            catch (EntityValidationException e)

            {
                HandleEntityValidationException(e);
            }
        }

        private static void HandleEntityValidationException(EntityValidationException e)
        {
            Console.WriteLine("Errors:");
            ConsoleEx.WriteLines(e.Errors, (value, i) => $"{i}: ");
        }
    }
}
