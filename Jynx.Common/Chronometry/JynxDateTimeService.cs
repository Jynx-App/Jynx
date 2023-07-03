using Jynx.Common.Abstractions.Chronometry;

namespace Jynx.Common.Chronometry
{
    internal class JynxDateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
