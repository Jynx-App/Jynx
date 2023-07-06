using Jynx.Abstractions.Entities.Auth;

namespace Jynx.Abstractions.Entities
{
    public class DistrictUser : BaseEntity
    {
        // The Id of DistrictUser should match the associated User

        public string DistrictId { get; set; } = "";

        public string? DistrictUserGroupId { get; set; }

        public HashSet<ModerationPermission> ModerationPermissions { get; set; } = new();

        public DateTime? BannedUntil { get; set; }

        public string? BanReason { get; set; }
    }
}
