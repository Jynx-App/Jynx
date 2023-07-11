namespace Jynx.Abstractions.Entities
{
    public class District : BaseEntity
    {
        public string Description { get; set; } = "";

        public PostsSortOrder DefaultPostSortOrder = PostsSortOrder.HighestScore;

        public PostsSortOrder DefaultCommentsSortOrder = PostsSortOrder.HighestScore;

        public List<string> PinnedPostIds { get; set; } = new List<string>();
    }
}
