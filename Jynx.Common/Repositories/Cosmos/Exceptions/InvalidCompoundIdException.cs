using Jynx.Common.ErrorHandling.Exceptions;
using System.Net;

namespace Jynx.Common.Repositories.Cosmos.Exceptions
{
    public class InvalidCompoundIdException : JynxException
    {
        public InvalidCompoundIdException(Exception? innerException = null)
            : base("Invalid Compound Id", null, innerException)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
