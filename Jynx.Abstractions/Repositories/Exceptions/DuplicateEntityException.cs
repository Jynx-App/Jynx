using Jynx.Abstractions.Exceptions;
using System.Net;

namespace Jynx.Abstractions.Repositories.Exceptions
{
    public class DuplicateEntityException : JynxException
    {
        public DuplicateEntityException(Exception? innerException = null)
            : base("Duplicate Entity", null, innerException)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
