using Jynx.Abstractions.Exceptions;

namespace Jynx.Abstractions.Repositories.Exceptions
{
    public class UnableToGenerateIdException : JynxException
    {
        public UnableToGenerateIdException(Exception? innerException = null)
            : base(null, null, innerException)
        {
        }
    }
}
