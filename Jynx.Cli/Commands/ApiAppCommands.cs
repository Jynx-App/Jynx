using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;

namespace Jynx.Cli.Commands
{
    [Command("api-app")]
    internal class ApiAppCommands : ConsoleAppBase
    {
        private readonly IApiAppService _apiAppService;

        public ApiAppCommands(IApiAppService apiAppService)
        {
            _apiAppService = apiAppService;
        }

        public async Task Create(string name, string ownerId)
        {
            var entity = new ApiApp
            {
                Name = name,
                UserId = ownerId
            };

            await _apiAppService.CreateAsync(entity);
        }
    }
}
