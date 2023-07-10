using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Responses
{
    public class GetCommentResponse : CommentModel
    {
        public GetCommentResponse(Comment post) : base(post)
        {
        }
    }
}
