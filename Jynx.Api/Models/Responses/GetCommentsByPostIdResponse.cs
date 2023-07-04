namespace Jynx.Api.Models.Responses
{
    public class GetCommentsByPostIdResponse
    {
        public IEnumerable<ReadCommentResponse> Comments { get; set; } = Array.Empty<ReadCommentResponse>();
    }
}
