using Infrastructure.Contexts;
using Infrastructure.Entities.Product;
using Infrastructure.Repositories.Product;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.Product
{
    public class CategoryRepository_Tests
    {
        private readonly ProductDbContext _context;
        private readonly CategoryRepository _repository;

        public CategoryRepository_Tests()
        {
            _context = new ProductDbContext(new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _repository = new CategoryRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddCategoryToDatabase()
        {
            // Arrange
            var category = new CategoryEntity
            {
                Name = "Category Test"
            };

            // Act
            var result = await _repository.AddAsync(category);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Id, result.Id);
            Assert.Equal(category.Name, result.Name);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteCategoryFromDatabase()
        {
            // Arrange
            var category = new CategoryEntity
            {
                Name = "Category Test"
            };

            await _repository.AddAsync(category);

            // Act
            var result = await _repository.DeleteAsync(c => c.Id == category.Id);
            var deletedCategory = await _repository.GetAsync(c => c.Id == category.Id);

            // Assert
            Assert.True(result);
            Assert.Null(deletedCategory);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllCategoriesFromDatabase()
        {
            // Arrange
            var categoriesToAdd = new List<CategoryEntity>
            {
                new() { Name = "Category Test 1" },
                new() { Name = "Category Test 2" },
                new() { Name = "Category Test 3" }
            };

            foreach (var category in categoriesToAdd)
            {
                await _repository.AddAsync(category);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAsync_ShouldReturnCategoryFromDatabase()
        {
            // Arrange
            var category = new CategoryEntity
            {
                Name = "Category Test"
            };

            await _repository.AddAsync(category);

            // Act
            var result = await _repository.GetAsync(c => c.Id == category.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(category.Id, result.Id);
            Assert.Equal(category.Name, result.Name);
        }
    }
}
