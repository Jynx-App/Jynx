using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Responses
{
    public class GetUserResponse
    {
        public GetUserResponse(User user)
        {
            Username = user.Username;
            Created = user.Created ?? DateTime.MinValue;
        }

        public string Username { get; set; } = "";

        public DateTime Created { get; set; }
    }
}
