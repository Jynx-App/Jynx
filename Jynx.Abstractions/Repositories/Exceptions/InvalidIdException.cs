using Jynx.Abstractions.Exceptions;
using System.Net;

namespace Jynx.Abstractions.Repositories.Exceptions
{
    public class InvalidIdException : JynxException
    {
        public InvalidIdException(Exception? innerException = null)
            : base("Invalid Id", null, innerException)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
