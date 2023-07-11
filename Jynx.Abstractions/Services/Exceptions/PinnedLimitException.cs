using Jynx.Abstractions.Exceptions;
using System.Net;

namespace Jynx.Abstractions.Services.Exceptions
{
    public class PinnedLimitException : JynxException
    {
        public PinnedLimitException(int limit, string entityName)
            : base($"Can not pin more than {limit} {entityName}", null, null)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
