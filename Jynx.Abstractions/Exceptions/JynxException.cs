﻿using System.Net;

namespace Jynx.Abstractions.Exceptions
{
    public class JynxException : Exception
    {
        public JynxException(string? safeMessage, string? message = null, Exception? innerException = null) : base(message ?? safeMessage, innerException)
        {
            SafeMessage = safeMessage;
        }

        public JynxExceptionSeverity Severity { get; set; } = JynxExceptionSeverity.Normal;

        public virtual string? SafeMessage { get; }

        public bool HasSafeMessage => !string.IsNullOrWhiteSpace(SafeMessage);

        public HttpStatusCode StatusCode { get; protected set; } = HttpStatusCode.InternalServerError;
    }
}
