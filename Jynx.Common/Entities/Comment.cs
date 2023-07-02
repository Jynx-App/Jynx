namespace Jynx.Common.Entities
{
    public class Comment : BaseEntity
    {
        public string PostId { get; set; } = "";

        public string UserId { get; set; } = "";

        public DateTime? Edited { get; set; }

        public string? EditedById { get; set; }

        public string Body { get; set; } = "";
    }
}
