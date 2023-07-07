using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Responses
{
    public class GetCommentResponse
    {
        public GetCommentResponse(Comment post)
        {
            Id = post.Id ?? "";
            Created = post.Created ?? DateTime.MinValue;
            Edited = post.Edited;
            DistrictId = post.DistrictId;
            PostId = post.PostId;
            ParentCommentId = post.ParentCommentId;
            UserId = post.UserId;
            EditedById = post.EditedById;
            Body = post.Body;
            UpVotes = post.UpVotes;
            DownVotes = post.DownVotes;
            TotalVotes = post.TotalVotes;
        }

        public string Id { get; set; } = "";

        public DateTime Created { get; set; }

        public DateTime? Edited { get; set; }

        public string DistrictId { get; set; } = "";

        public string PostId { get; set; } = "";

        public string? ParentCommentId { get; set; }

        public string UserId { get; set; } = "";

        public string? EditedById { get; set; }

        public string Body { get; set; }

        public int UpVotes { get; set; }

        public int DownVotes { get; set; }

        public int TotalVotes { get; set; }
    }
}
