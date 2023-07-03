using Jynx.Common.Entities;
using System.ComponentModel.DataAnnotations;

namespace Jynx.Api.Models.Requests
{
    public class RegisterUserRequest
    {
        [StringLength(32)]
        [RegularExpression("^[a-zA-Z][a-zA-Z0-9_-]+$")]
        public string Username { get; set; } = "";
        
        [StringLength(320)]
        public string Email { get; set; } = "";

        [StringLength(1024, MinimumLength = 8)]
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
