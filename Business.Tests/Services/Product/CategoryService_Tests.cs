using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities.Product;
using Infrastructure.Repositories.Product;
using Microsoft.EntityFrameworkCore;

namespace Business.Tests.Services.Product
{
    public class CategoryService_Tests
    {
        private readonly ProductDbContext _context;
        private readonly CategoryService _categoryService;
        public CategoryService_Tests()
        {
            _context = new(new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            var categoryRepository = new CategoryRepository(_context);
            _categoryService = new CategoryService(categoryRepository);
        }

        [Fact]
        public async Task GetOrCreateCategoryAsync_ExistingCategory_ShouldReturnExistingCategory()
        {
            // Arrange
            var newCategory = new CategoryEntity { Name = " Test Category" };
            await _categoryService.GetOrCreateCategoryAsync(newCategory.Name);

            // Act
            var existingCategory = new CategoryEntity { Name = " Test Category" };
            var result = await _categoryService.GetOrCreateCategoryAsync(existingCategory.Name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newCategory.Name, result.Name);
        }

        [Fact]
        public async Task GetOrCreateCategoryAsync_NewCategory_ShouldCreateAndReturnNewCategory()
        {
            // Arrange
            var newCategory = new CategoryEntity { Name = " Test Category" };
            await _categoryService.GetOrCreateCategoryAsync(newCategory.Name);

            // Act
            var existingCategory = new CategoryEntity { Name = " Test Category 2" };
            var result = await _categoryService.GetOrCreateCategoryAsync(existingCategory.Name);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(newCategory.Name, result.Name);
        }
    }
}
