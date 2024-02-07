using Infrastructure.Contexts;
using Infrastructure.Entities.Product;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Repositories.Product
{
    public class ProductRepository(ProductDbContext dbContext) : BaseRepository<ProductEntity, ProductDbContext>(dbContext)
    {
        public override async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            try
            {
                var entities = await _dbContext.Set<ProductEntity>()
                    .Include(product => product.Category)
                    .Include(product => product.Manufacturer)
                    .Include(product => product.Inventories)
                    .ToListAsync();

                return entities;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Enumerable.Empty<ProductEntity>();
            }
        }
    }
}
