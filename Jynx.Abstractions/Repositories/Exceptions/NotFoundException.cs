using Jynx.Abstractions.Exceptions;
using System.Net;

namespace Jynx.Abstractions.Repositories.Exceptions
{
    public class NotFoundException : JynxException
    {
        public NotFoundException(string name)
            : base($"{name} not found", null, null)
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}
