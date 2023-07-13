using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Requests
{
    public class UpdateCommentRequest : DistrictRelatedIdRequest, ICanPatch<Comment>
    {
        public string Body { get; set; } = "";

        void ICanPatch<Comment>.Patch(Comment entity)
        {
            entity.Body = Body;
        }
    }
}
