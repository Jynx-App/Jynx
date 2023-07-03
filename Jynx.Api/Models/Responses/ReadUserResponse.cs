using Jynx.Common.Entities;

namespace Jynx.Api.Models.Responses
{
    public class ReadUserResponse
    {
        public ReadUserResponse(User user)
        {
            Username = user.Username;
        }

        public string Username { get; set; } = "";
    }
}
