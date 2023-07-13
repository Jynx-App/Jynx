using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models
{
    public class PostModel
    {
        public PostModel(Post post)
        {
            Id = post.Id ?? "";
            Created = post.Created ?? DateTime.MinValue;
            DistrictId = post.DistrictId;
            UserId = post.UserId;
            Edited = post.Edited;
            EditedById = post.EditedById;
            Title = post.Title;
            Body = post.Body;
            Url = post.Url;
            CommentsLocked = post.CommentsLocked;
            UpVotes = post.UpVotes;
            DownVotes = post.DownVotes;
            TotalVotes = post.TotalVotes;
            Score = post.Score;
            Pinned = post.Pinned;
            Comments = post.Comments;
        }

        public string Id { get; set; } = "";

        public DateTime Created { get; set; }

        public DateTime? Edited { get; set; }

        public string DistrictId { get; set; } = "";

        public string UserId { get; set; } = "";

        public string? EditedById { get; set; }

        public string Title { get; set; } = "";

        public string? Body { get; set; }

        public string? Url { get; set; }

        public bool CommentsLocked { get; set; }

        public int UpVotes { get; set; }

        public int DownVotes { get; set; }

        public int TotalVotes { get; set; }

        public int Score { get; set; }

        public DateTime? Pinned { get; set; }

        public int Comments { get; set; }
    }
}
