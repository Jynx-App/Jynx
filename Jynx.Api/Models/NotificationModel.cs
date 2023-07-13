using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models
{
    public class NotificationModel
    {
        public NotificationModel(Notification entity)
        {
            Id = entity.Id;
            Created = entity.Created ?? DateTime.MinValue;
            Edited = entity.Edited ?? DateTime.MinValue;
            UserId = entity.UserId;
            Title = entity.Title;
            Body = entity.Body;
            CommentId = entity.CommentId;
            Read = entity.Read;
        }

        public string Id { get; set; } = "";

        public DateTime Created { get; set; }

        public DateTime Edited { get; set; }

        public string UserId { get; set; } = "";

        public string Title { get; set; } = "";

        public string? Body { get; set; }

        public string? CommentId { get; set; }

        public bool Read { get; set; }
    }
}
