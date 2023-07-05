using ConsoleAppFramework;
using Jynx.Cli.Commands.RepositoryServices;
using Jynx.Common;
using Jynx.Common.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;

namespace Jynx.Cli
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (Debugger.IsAttached)
                args = GetDebuggerArgs();

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
            app.AddSubCommands<ApiAppUserCommands>();
            app.AddSubCommands<CommentCommands>();
            app.AddSubCommands<DistrictCommands>();
            app.AddSubCommands<DistrictUserCommands>();
            app.AddSubCommands<DistrictUserGroupCommands>();
            app.AddSubCommands<NotificationCommands>();
            app.AddSubCommands<PostCommands>();
            app.AddSubCommands<UserCommands>();

            await app.RunAsync();
        }

        private static string[] GetDebuggerArgs()
            => Array.Empty<string>();

        private static string[] SplitCommandLineIntoArguments(string commandLine)
        {
            var chars = commandLine.ToCharArray();

            var inQuote = false;

            for (var i = 0; i < chars.Length; i++)
            {
                if (chars[i] == '"')
                    inQuote = !inQuote;

                if (!inQuote && chars[i] == ' ')
                    chars[i] = '\n';
            }

            return (new string(chars)).Split('\n');
        }
    }
}