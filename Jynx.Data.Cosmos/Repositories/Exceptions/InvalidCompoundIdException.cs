using Jynx.Abstractions.Exceptions;
using System.Net;

namespace Jynx.Data.Cosmos.Repositories.Exceptions
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
