using Jynx.Common.ErrorHandling.Exceptions;
using System.Net;

namespace Jynx.Common.Repositories.Exceptions
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
