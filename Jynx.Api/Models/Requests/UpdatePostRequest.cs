using Jynx.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace Jynx.Api.Models.Requests
{
    public class UpdatePostRequest : ICanPatch<Post>
    {
        public string Id { get; set; } = "";

        public string Body { get; set; } = "";

        void ICanPatch<Post>.Patch(Post entity)
        {
            entity.Body = Body;
        }
    }
}
