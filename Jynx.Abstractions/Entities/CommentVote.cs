namespace Jynx.Abstractions.Entities
{
    public class CommentVote : BaseEntity
    {
        // The Id of CommentVote should match the associated User

        public string CommentId { get; set; } = "";

        public bool Up { get; set; }
    }
}
