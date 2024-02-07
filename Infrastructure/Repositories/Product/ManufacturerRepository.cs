using Infrastructure.Contexts;
using Infrastructure.Entities.Product;

namespace Infrastructure.Repositories.Product
{
    public class ManufacturerRepository(ProductDbContext dbContext) : BaseRepository<ManufacturerEntity, ProductDbContext>(dbContext)
    {
    }
}
