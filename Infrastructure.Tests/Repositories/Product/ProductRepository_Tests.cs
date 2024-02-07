using Infrastructure.Contexts;
using Infrastructure.Entities.Product;
using Infrastructure.Repositories.Product;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.Product
{
    public class ProductRepository_Tests
    {
        private readonly ProductDbContext _context;
        private readonly ProductRepository _repository;

        public ProductRepository_Tests()
        {
            _context = new ProductDbContext(new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _repository = new ProductRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddProductToDatabase()
        {
            // Arrange
            var category = new CategoryEntity { Name = "Test Category" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            var manufacturer = new ManufacturerEntity { Name = "Test Manufacturer" };
            await _context.Manufacturers.AddAsync(manufacturer);
            await _context.SaveChangesAsync();

            var product = new ProductEntity
            {
                Name = "Test Product",
                Price = 10.50m,
                CategoryId = category.Id,
                ManufacturerId = manufacturer.Id
            };

            // Act
            var result = await _repository.AddAsync(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.CategoryId, result.CategoryId);
            Assert.Equal(product.ManufacturerId, result.ManufacturerId);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteProductFromDatabase()
        {
            // Arrange
            var category = new CategoryEntity { Name = "Test Category" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            var manufacturer = new ManufacturerEntity { Name = "Test Manufacturer" };
            await _context.Manufacturers.AddAsync(manufacturer);
            await _context.SaveChangesAsync();

            var product = new ProductEntity
            {
                Name = "Test Product",
                Price = 10.50m,
                CategoryId = category.Id,
                ManufacturerId = manufacturer.Id
            };

            await _repository.AddAsync(product);

            // Act
            var result = await _repository.DeleteAsync(p => p.Id == product.Id);
            var deletedProduct = await _repository.GetAsync(p => p.Id == product.Id);

            // Assert
            Assert.True(result);
            Assert.Null(deletedProduct);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProductsFromDatabase()
        {
            // Arrange
            var category = new CategoryEntity { Name = "Test Category" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            var manufacturer = new ManufacturerEntity { Name = "Test Manufacturer" };
            await _context.Manufacturers.AddAsync(manufacturer);
            await _context.SaveChangesAsync();

            var productsToAdd = new List<ProductEntity>
            {
                new() { Name = "Product Test 1", Price = 10.50m, CategoryId = category.Id, ManufacturerId = manufacturer.Id },
                new() { Name = "Product Test 2", Price = 20.50m, CategoryId = category.Id, ManufacturerId = manufacturer.Id },
                new() { Name = "Product Test 3", Price = 30.50m, CategoryId = category.Id, ManufacturerId = manufacturer.Id }
            };

            foreach (var product in productsToAdd)
            {
                await _repository.AddAsync(product);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAsync_ShouldReturnProductFromDatabase()
        {
            // Arrange
            var category = new CategoryEntity { Name = "Test Category" };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            var manufacturer = new ManufacturerEntity { Name = "Test Manufacturer" };
            await _context.Manufacturers.AddAsync(manufacturer);
            await _context.SaveChangesAsync();

            var product = new ProductEntity
            {
                Name = "Test Product",
                Price = 10.50m,
                CategoryId = category.Id,
                ManufacturerId = manufacturer.Id
            };

            await _repository.AddAsync(product);

            // Act
            var result = await _repository.GetAsync(p => p.Id == product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.CategoryId, result.CategoryId);
            Assert.Equal(product.ManufacturerId, result.ManufacturerId);
        }
    }
}