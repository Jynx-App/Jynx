namespace Jynx.Common.Entities
{
    public abstract class BaseEntity
    {
        public string? Id { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Edited { get; set; }
    }
}
