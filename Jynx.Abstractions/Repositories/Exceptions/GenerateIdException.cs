using Jynx.Abstractions.Exceptions;

namespace Jynx.Abstractions.Repositories.Exceptions
{
    public class GenerateIdException : JynxException
    {
        public GenerateIdException(Exception? innerException = null)
            : base(null, null, innerException)
        {
        }
    }
}
