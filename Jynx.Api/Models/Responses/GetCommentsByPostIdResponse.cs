namespace Jynx.Api.Models.Responses
{
    public class GetCommentsByPostIdResponse
    {
        public IEnumerable<GetCommentResponse> Comments { get; set; } = Array.Empty<GetCommentResponse>();
    }
}
