using Jynx.Common.Entities;

namespace Jynx.Api.Models.Requests
{
    public class UpdatePostRequest
    {
        public string Id { get; set; } = "";

        public string Body { get; set; } = "";

        public void PatchEntity(Post post)
        {
            post.Body = Body;
        }
    }
}
