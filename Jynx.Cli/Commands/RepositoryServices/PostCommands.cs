using ConsoleAppFramework;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Entities;

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
