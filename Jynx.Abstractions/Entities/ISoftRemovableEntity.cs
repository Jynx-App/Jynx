namespace Jynx.Abstractions.Entities
{
    public interface ISoftRemovableEntity
    {
        public DateTime? Removed { get; set; }
    }
}
