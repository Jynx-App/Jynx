namespace Jynx.Common.Entities
{
    public class Post : BaseEntity
    {
        public string UserId { get; set; } = "";

        public string? EditedById { get; set; }

        public string Title { get; set; } = "";

        public string Body { get; set; } = "";
    }
}
