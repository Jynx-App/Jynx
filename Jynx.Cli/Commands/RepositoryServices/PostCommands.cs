using ConsoleAppFramework;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
