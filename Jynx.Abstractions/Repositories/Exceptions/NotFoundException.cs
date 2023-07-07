using Jynx.Abstractions.Exceptions;
using System.Net;

namespace Jynx.Abstractions.Repositories.Exceptions
{
    public class NotFoundException : JynxException
    {
        public NotFoundException(string name, Exception? innerException = null)
            : base($"{name} not found", null, innerException)
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}
