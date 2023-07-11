namespace Jynx.Common.Events
{
    public interface IEventSubscriber<TEvent>
    {
        Task HandleAsync(object sender, TEvent @event);
    }
}
