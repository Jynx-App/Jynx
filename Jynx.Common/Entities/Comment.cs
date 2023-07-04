namespace Jynx.Common.Entities
{
    public class Comment : BaseEntity
    {
        public string DistrictId { get; set; } = "";

        public string PostId { get; set; } = "";

        public string? ParentCommentId { get; set; }

        public string UserId { get; set; } = "";

        public string? EditedById { get; set; }

        public string Body { get; set; } = "";
    }
}
