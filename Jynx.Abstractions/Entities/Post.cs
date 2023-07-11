using System.Text.Json.Serialization;

namespace Jynx.Abstractions.Entities
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

        public int UpVotes { get; set; }

        public int DownVotes { get; set; }

        public int Score => UpVotes - DownVotes;

        public int TotalVotes => UpVotes + DownVotes;

        public PostsSortOrder DefaultCommentsSortOrder { get; set; }

        [JsonIgnore]
        public bool Pinned { get; set; }
    }
}
