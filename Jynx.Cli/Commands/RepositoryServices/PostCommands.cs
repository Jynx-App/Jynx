using ConsoleAppFramework;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("posts")]
    internal class PostCommands : RepositoryServiceCommands<IPostsService, Post>
    {
        public PostCommands(IPostsService service)
            : base(service)
        {
        }
    }
}
