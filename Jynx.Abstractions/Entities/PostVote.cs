namespace Jynx.Abstractions.Entities
{
    public class PostVote : BaseEntity
    {
        // The Id of PostVote should match the associated User

        public string PostId { get; set; } = "";

        public bool Up { get; set; }
    }
}
