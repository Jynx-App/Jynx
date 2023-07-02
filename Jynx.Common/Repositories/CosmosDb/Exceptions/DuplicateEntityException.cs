using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jynx.Common.Repositories.CosmosDb.Exceptions
{
    internal class DuplicateEntityException : Exception
    {
        public DuplicateEntityException()
        {
        }

        public DuplicateEntityException(string? message) : base(message)
        {
        }

        public DuplicateEntityException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected DuplicateEntityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
