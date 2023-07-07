using Jynx.Abstractions.Exceptions;
using System.Net;

namespace Jynx.Abstractions.Services.Exceptions
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

        public override string? SafeMessage => string.Join('\n', Errors);
    }
}
