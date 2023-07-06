namespace Jynx.Abstractions.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = "";

        public string Email { get; set; } = "";

        public string Password { get; set; } = "";

        public DateTime? LastLogin { get; set; }

        public DateTime? BannedUntil { get; set; }

        public string? BanReason { get; set; }
    }
}
