namespace Jynx.Common.Abstractions.Chronometry
{
    public interface IDateTimeService
    {
        public DateTime Now { get; }
        public DateTime UtcNow { get; }
    }
}
