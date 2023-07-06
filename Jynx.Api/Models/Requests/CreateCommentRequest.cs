using Jynx.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace Jynx.Api.Models.Requests
{
    public class CreateCommentRequest
    {
        public string DistrictId { get; set; } = "";

        public string PostId { get; set; } = "";

        public string? ParentCommentId { get; set; }

        public string Body { get; set; } = "";

        public Comment ToEntity()
            => new()
            {
                DistrictId = DistrictId,
                PostId = PostId,
                ParentCommentId = ParentCommentId,
                Body = Body,
            };
    }
}
