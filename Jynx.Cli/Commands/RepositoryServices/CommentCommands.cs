﻿using ConsoleAppFramework;
using Jynx.Abstractions.Entities;
using Jynx.Abstractions.Services;

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
