using Jynx.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace Jynx.Api.Models.Requests
{
    public class UpdatePostRequest : ICanPatch<Post>
    {
        [StringLength(70)]
        public string Id { get; set; } = "";

        [StringLength(40000)]
        public string Body { get; set; } = "";

        void ICanPatch<Post>.Patch(Post entity)
        {
            entity.Body = Body;
        }
    }
}
