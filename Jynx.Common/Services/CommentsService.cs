using Jynx.Common.Abstractions.Repositories;
using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Microsoft.Extensions.Logging;

namespace Jynx.Common.Services
{
    internal class CommentsService : RepositoryService<ICommentsRepository, Comment>, ICommentsService
    {
        public CommentsService(
            ICommentsRepository repository,
            ILogger logger)
            : base(repository, logger)
        {
        }
    }
}
