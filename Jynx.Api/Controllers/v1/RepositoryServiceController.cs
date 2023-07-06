using Jynx.Common.Abstractions.Services;
using Jynx.Common.Entities;
using Jynx.Common.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Jynx.Api.Controllers.v1
{
    public class RepositoryServiceController<TRepositoryService, TEntity> : BaseController
        where TRepositoryService : IRepositoryService<TEntity>
        where TEntity : BaseEntity
    {
        protected static string DefaultNotFoundMessage { get; } = $"{typeof(TEntity)} not found";

        public RepositoryServiceController(
            TRepositoryService repositoryService,
            ILogger logger)
            : base(logger)
        {
            RepositoryService = repositoryService;
        }

        public TRepositoryService RepositoryService { get; }

        protected IActionResult ValidationError(EntityValidationException ex)
        {
            var errorsMessages = ex.Errors.ToList();

            if (!errorsMessages.Any())
                errorsMessages.Add("Missing json in request body");

            return MultiError(errorsMessages);
        }
    }
}
