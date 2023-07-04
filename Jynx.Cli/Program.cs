using Jynx.Cli.Commands;
using Jynx.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jynx.Cli
{
    internal class Program
    {
        private const string _defaultCommand = "api-app create --name Test --owner-id Test";

        static async Task Main(string[] args)
        {
#if DEBUG
            args = _defaultCommand.Split(' ');
#endif

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
            };

            Console.WriteLine($"Jynx.Cli {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}");

            var builder = ConsoleApp.CreateBuilder(args)
                .ConfigureAppConfiguration(cb =>
                {
                    cb.AddUserSecrets<Program>();
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddCommon(context.Configuration)
                        .AddSingleton<ISystemClock, SystemClock>();
                });
            
            var app = builder.Build();

            app.AddSubCommands<ApiAppCommands>();

            await app.RunAsync();
        }
    }
}