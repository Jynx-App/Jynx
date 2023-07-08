using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;
using Jynx.Cli.Commands.RepositoryServices;

namespace Jynx.Cli.Commands.EntityServices
{
    [Command("post-votes")]
    internal class PostVotesCommands : EntityServiceCommands<IPostVotesService, PostVote>
    {
        public PostVotesCommands(IPostVotesService service)
            : base(service)
        {
        }
    }
}
