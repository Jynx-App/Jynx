using ConsoleAppFramework;
using Jynx.Abstractions.Services;
using Jynx.Abstractions.Entities;

namespace Jynx.Cli.Commands.RepositoryServices
{
    [Command("comments")]
    internal class CommentCommands : RepositoryServiceCommands<ICommentsService, Comment>
    {
        public CommentCommands(ICommentsService service)
            : base(service)
        {
        }
    }
}
