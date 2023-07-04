using Jynx.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace Jynx.Api.Models.Requests
{
    public class CreateCommentRequest
    {
        [StringLength(100)]
        public string DistrictId { get; set; } = "";

        [StringLength(100)]
        public string PostId { get; set; } = "";

        [StringLength(1000)]
        public string? ParentCommentId { get; set; }

        [StringLength(40000)]
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
