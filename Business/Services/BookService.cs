using Infrastructure.Entities;
using Infrastructure.Interfaces;

namespace Infrastructure.Services
{
    public class BookService : IProductService<BookEntity>
    {
        public bool AddEntity(object newEntity)
        {
            throw new NotImplementedException();
        }

        public bool GetAll(out IEnumerable<object> result)
        {
            throw new NotImplementedException();
        }

        public bool RemoveEntity(int entityId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateEntity(int entityId, object updatedEntity)
        {
            throw new NotImplementedException();
        }
    }
}
