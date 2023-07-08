using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("comments")]
    internal class CommentsCommands : EntityServiceCommands<ICommentsService, Comment>
    {
        public CommentsCommands(ICommentsService service)
            : base(service)
        {
        }
    }
}
