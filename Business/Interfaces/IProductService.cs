
namespace Infrastructure.Interfaces
{
    public interface IProductService<TEntity> where TEntity : class
    {
        bool GetAll(out IEnumerable<object> result);
        bool UpdateEntity(int entityId, object updatedEntity);
        bool AddEntity(object newEntity);
        bool RemoveEntity(int entityId);
    }
}
