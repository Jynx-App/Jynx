using ConsoleAppFramework;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;

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
