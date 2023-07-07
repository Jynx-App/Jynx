using Jynx.Common.ErrorHandling.Exceptions;
using System.Net;

namespace Jynx.Data.Cosmos.Repositories.Exceptions
{
    public class MissingPartitionKeyException : JynxException
    {
        public MissingPartitionKeyException(Exception? innerException = null)
            : base("Missing Partition Key", null, innerException)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}
