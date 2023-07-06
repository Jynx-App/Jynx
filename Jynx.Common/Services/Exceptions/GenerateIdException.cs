using Jynx.Common.ErrorHandling.Exceptions;

namespace Jynx.Common.Services.Exceptions
{
    public class GenerateIdException : JynxException
    {
        public GenerateIdException(Exception? innerException = null)
            : base(null, null, innerException)
        {
        }
    }
}
