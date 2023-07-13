using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Responses
{
    public class GetNotificationsResponse
    {
        public IEnumerable<NotificationModel> Notifications { get; set; } = Array.Empty<NotificationModel>();
    }
}
