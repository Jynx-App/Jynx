using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;
using Jynx.Cli.Commands.RepositoryServices;

namespace Jynx.Cli.Commands.EntityServices
{
    [Command("post-votes")]
    internal class PostVoteCommands : EntityServiceCommands<IPostVotesService, PostVote>
    {
        public PostVoteCommands(IPostVotesService service)
            : base(service)
        {
        }
    }
}
