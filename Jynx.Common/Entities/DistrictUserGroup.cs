using Jynx.Common.Auth;

namespace Jynx.Common.Entities
{
    public class DistrictUserGroup : BaseEntity
    {
        public string DistrictId { get; set; } = "";

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public HashSet<ModerationPermission> ModerationPermissions { get; set; } = new();
    }
}
