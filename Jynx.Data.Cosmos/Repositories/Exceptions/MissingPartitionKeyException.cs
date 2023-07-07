using Jynx.Common.ErrorHandling.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
