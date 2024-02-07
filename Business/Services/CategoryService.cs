using Infrastructure.Entities.Product;
using Infrastructure.Repositories.Product;

namespace Business.Services
{
    public class CategoryService(CategoryRepository categoryRepository)
    {
        private readonly CategoryRepository _categoryRepository = categoryRepository;

        public async Task<CategoryEntity> GetOrCreateCategoryAsync(string categoryName)
        {
            var existingCategory = await _categoryRepository.GetAsync(category => category.Name == categoryName);

            if (existingCategory != null)
            {
                return existingCategory;
            }

            var newCategory = await _categoryRepository.AddAsync(new CategoryEntity { Name = categoryName });
            return newCategory;
        }
    }
}
