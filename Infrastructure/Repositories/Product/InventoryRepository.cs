using Infrastructure.Contexts;
using Infrastructure.Entities.Product;

namespace Infrastructure.Repositories.Product
{
    public class InventoryRepository(ProductDbContext dbContext) : ProductBaseRepository<InventoryEntity>(dbContext)
    {
    }
}
