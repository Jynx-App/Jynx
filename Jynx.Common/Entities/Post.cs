namespace Jynx.Common.Entities
{
    public class Post : BaseEntity, ISoftRemovableEntity
    {
        public string DistrictId { get; set; } = "";

        public string UserId { get; set; } = "";

        public string? EditedById { get; set; }

        public string Title { get; set; } = "";

        public string? Body { get; set; }

        public string? Url { get; set; }

        public bool CommentsLocked { get; set; }
        public DateTime? Removed { get; set; }
    }
}
