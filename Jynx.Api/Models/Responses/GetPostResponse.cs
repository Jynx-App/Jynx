using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Responses
{
    public class GetPostResponse : PostModel
    {
        public GetPostResponse(Post post) : base(post)
        {
        }
    }
}
