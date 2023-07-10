using Microsoft.Extensions.DependencyInjection;

namespace Jynx.Common.Events
{
    public static class EventExtensions
    {
        public static IServiceCollection AddEventPublisher(this IServiceCollection services)
        {
            services.AddSingleton<IEventPublisher, EventPublisher>();

            return services;
        }

        public static IEnumerable<IEventSubscriber<TEvent>> GetEventSubscribers<TEvent>(this IServiceProvider services)
        {
            var eventSubscribers = services.GetServices<IEventSubscriber<TEvent>>();

            return eventSubscribers;
        }
    }
}
