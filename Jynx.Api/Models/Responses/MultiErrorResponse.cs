namespace Jynx.Api.Models.Responses
{
    public class MultiErrorResponse
    {
        public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();

        public string RequestId { get; set; } = "";
    }
}
