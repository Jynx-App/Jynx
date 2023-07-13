namespace Jynx.Abstractions.Entities
{
    public class Comment : BaseEntity, ISoftRemovableEntity
    {
        public string DistrictId { get; set; } = "";

        public string PostId { get; set; } = "";

        public string? ParentCommentId { get; set; }

        public string UserId { get; set; } = "";

        public string? EditedById { get; set; }

        public string Body { get; set; } = "";

        public int UpVotes { get; set; }

        public int DownVotes { get; set; }

        public int Score => UpVotes - DownVotes;

        public int TotalVotes => UpVotes + DownVotes;

        public DateTime? Pinned { get; set; }

        public DateTime? Removed { get; set; }
    }
}
