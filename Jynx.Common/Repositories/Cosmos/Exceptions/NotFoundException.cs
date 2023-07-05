﻿using Jynx.Common.ErrorHandling.Exceptions;
using System.Net;

namespace Jynx.Common.Repositories.Cosmos.Exceptions
{
    public class NotFoundException : JynxException
    {
        public NotFoundException(Exception? innerException = null)
            : base("Not Found", null, innerException)
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}