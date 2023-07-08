using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("posts")]
    internal class PostsCommands : EntityServiceCommands<IPostsService, Post>
    {
        public PostsCommands(IPostsService service)
            : base(service)
        {
        }
    }
}
