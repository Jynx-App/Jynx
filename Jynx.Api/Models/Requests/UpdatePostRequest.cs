using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Requests
{
    public class UpdatePostRequest : DistrictRelatedIdRequest, ICanPatch<Post>
    {
        public string Body { get; set; } = "";

        void ICanPatch<Post>.Patch(Post entity)
        {
            entity.Body = Body;
        }
    }
}
