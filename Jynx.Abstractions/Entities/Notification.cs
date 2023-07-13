namespace Jynx.Abstractions.Entities
{
    public class Notification : BaseEntity
    {
        public NotificationType Type { get; set; }

        public string UserId { get; set; } = "";

        public string Title { get; set; } = "";

        public string? Body { get; set; }

        public string? ForeignId { get; set; }

        public bool Read { get; set; }
    }
}
