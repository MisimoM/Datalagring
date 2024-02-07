using Infrastructure.Contexts;
using Infrastructure.Entities.Book;
using Infrastructure.Entities.Product;

namespace Infrastructure.Repositories.Product
{
    public class CategoryRepository(ProductDbContext dbContext) : BaseRepository<CategoryEntity, ProductDbContext>(dbContext)
    {
    }
}
