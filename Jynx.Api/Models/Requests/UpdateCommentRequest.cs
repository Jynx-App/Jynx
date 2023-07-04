using Jynx.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace Jynx.Api.Models.Requests
{
    public class UpdateCommentRequest : ICanPatch<Comment>
    {
        [StringLength(100)]
        public string Id { get; set; } = "";

        [StringLength(40000)]
        public string Body { get; set; } = "";

        void ICanPatch<Comment>.Patch(Comment entity)
        {
            entity.Body = Body;
        }
    }
}
