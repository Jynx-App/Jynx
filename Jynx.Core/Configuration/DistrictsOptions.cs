namespace Jynx.Core.Configuration
{
    public class DistrictsOptions
    {
        public const string DefaultKey = "Districts";

        public int MaxPinnedPosts { get; set; } = 2;

        public int MaxPinnedComments { get; set; } = 1;
    }
}
