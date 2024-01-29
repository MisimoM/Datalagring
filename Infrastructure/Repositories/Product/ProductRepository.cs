using Infrastructure.Contexts;
using Infrastructure.Entities.Product;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Repositories.Product
{
    public class ProductRepository(ProductDbContext dbContext) : ProductBaseRepository<ProductEntity>(dbContext)
    {
        public override async Task<IEnumerable<ProductEntity>> GetAllAsync()
        {
            try
            {
                var entities = await _dbContext.Set<ProductEntity>()
                    .Include(product => product.Category)
                    .Include(product => product.Manufacturer)
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
