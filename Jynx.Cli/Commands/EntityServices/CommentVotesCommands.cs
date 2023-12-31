﻿using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;
using Jynx.Cli.Commands.RepositoryServices;

namespace Jynx.Cli.Commands.EntityServices
{
    [Command("comment-votes")]
    internal class CommentVotesCommands : EntityServiceCommands<ICommentVotesService, CommentVote>
    {
        public CommentVotesCommands(ICommentVotesService service)
            : base(service)
        {
        }
    }
}
