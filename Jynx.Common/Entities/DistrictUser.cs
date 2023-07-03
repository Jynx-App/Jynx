using Jynx.Common.Auth;

namespace Jynx.Common.Entities
{
    public class DistrictUser : BaseEntity
    {
        public string DistrictId { get; set; } = "";

        public string? DistrictUserGroupId { get; set; }

        public HashSet<ModerationPermission> ModerationPermissions { get; set; } = new();

        public DateTime? BannedUntil { get; set; }

        public string? BanReason { get; set; }
    }
}
