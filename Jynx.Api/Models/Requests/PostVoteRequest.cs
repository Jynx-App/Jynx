namespace Jynx.Api.Models.Requests
{
    public class PostVoteRequest
    {
        public string PostId { get; set; } = "";

        public bool Negative { get; set; }
    }
}
