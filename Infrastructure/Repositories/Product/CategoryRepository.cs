﻿using Infrastructure.Contexts;
using Infrastructure.Entities.Product;

namespace Infrastructure.Repositories.Product
{
    public class CategoryRepository(ProductDbContext dbContext) : ProductBaseRepository<CategoryEntity>(dbContext)
    {
    }
}