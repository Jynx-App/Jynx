namespace Jynx.Common.ErrorHandling.Exceptions
{
    public class JynxException : Exception
    {
        public JynxException(string? safeMessage, string? message = null, Exception? innerException = null) : base(message ?? safeMessage, innerException)
        {
            SafeMessage = safeMessage;
        }

        public JynxExceptionSeverity Severity { get; set; } = JynxExceptionSeverity.Normal;

        public string? SafeMessage { get; set; }

        public bool HasSafeMessage => !string.IsNullOrWhiteSpace(SafeMessage);
    }
}
