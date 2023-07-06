namespace Jynx.Abstractions.Entities
{
    public interface ICanPatch<TEntity>
        where TEntity : BaseEntity
    {
        public void Patch(TEntity entity);
    }
}
