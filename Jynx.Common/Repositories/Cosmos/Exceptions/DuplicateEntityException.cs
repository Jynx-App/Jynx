using Jynx.Common.ErrorHandling.Exceptions;
using System.Net;

namespace Jynx.Common.Repositories.Cosmos.Exceptions
{
    internal class DuplicateEntityException : JynxException
    {
        public DuplicateEntityException(Exception? innerException = null)
            : base("Duplicate Entity", null, innerException)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
