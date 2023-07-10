using Microsoft.Extensions.DependencyInjection;

namespace Jynx.Common.Events
{
    internal class EventPublisher : IEventPublisher
    {
        private readonly IServiceProvider _services;

        public EventPublisher(IServiceProvider services)
        {
            _services = services;
        }

        public async Task PublishAsync<TEvent>(object sender, TEvent @event)
        {
            using var scope = _services.CreateAsyncScope();

            var handlers = scope.ServiceProvider.GetEventSubscribers<TEvent>();

            foreach(var handler in handlers)
            {
                await handler.HandleAsync(sender, @event);
            }
        }
    }
}
