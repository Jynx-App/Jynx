﻿using Jynx.Common.ErrorHandling.Exceptions;
using System.Net;

namespace Jynx.Common.Repositories.Exceptions
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