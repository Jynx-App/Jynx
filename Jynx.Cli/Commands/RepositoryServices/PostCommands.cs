using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

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
