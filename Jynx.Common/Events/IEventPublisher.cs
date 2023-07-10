namespace Jynx.Common.Events
{
    public interface IEventPublisher
    {
        public Task PublishAsync<TEvent>(object sender, TEvent @event);
    }
}
