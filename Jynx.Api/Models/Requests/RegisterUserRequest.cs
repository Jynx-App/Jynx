using Jynx.Abstractions.Entities;

namespace Jynx.Api.Models.Requests
{
    public class RegisterUserRequest
    {
        public string Username { get; set; } = "";

        public string Email { get; set; } = "";

        public string Password { get; set; } = "";

        public User ToEntity()
            => new()
            {
                Username = Username,
                Email = Email,
                Password = Password
            };
    }
}
