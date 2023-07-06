using Jynx.Common.ErrorHandling.Exceptions;
using System.Net;

namespace Jynx.Common.Services.Exceptions
{
    public class EntityValidationException : JynxException
    {
        public EntityValidationException(string entityName, IEnumerable<string> errors)
            : base($"{entityName} validation failure", null, null)
        {
            StatusCode = HttpStatusCode.BadRequest;
            Errors = errors;
        }

        public IEnumerable<string> Errors { get; }
    }
}
