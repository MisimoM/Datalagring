using Infrastructure.Contexts;
using Infrastructure.Entities.Product;
using Infrastructure.Repositories.Product;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.Product
{
    public class InventoryRepository_Tests
    {
        private readonly ProductDbContext _context;
        private readonly InventoryRepository _repository;

        public InventoryRepository_Tests()
        {
            _context = new ProductDbContext(new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _repository = new InventoryRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddInventoryToDatabase()
        {
            // Arrange
            var product = new ProductEntity { Name = "Test Product", Price = 10.50m };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var inventory = new InventoryEntity
            {
                Quantity = 100,
                ProductId = product.Id
            };

            // Act
            var result = await _repository.AddAsync(inventory);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(inventory.Id, result.Id);
            Assert.Equal(inventory.Quantity, result.Quantity);
            Assert.Equal(inventory.ProductId, result.ProductId);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteInventoryFromDatabase()
        {
            // Arrange
            var product = new ProductEntity { Name = "Test Product", Price = 10.50m };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var inventory = new InventoryEntity
            {
                Quantity = 100,
                ProductId = product.Id
            };

            await _repository.AddAsync(inventory);

            // Act
            var result = await _repository.DeleteAsync(i => i.Id == inventory.Id);

            // Assert
            Assert.True(result);
            var deletedInventory = await _repository.GetAsync(i => i.Id == inventory.Id);
            Assert.Null(deletedInventory);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllInventoriesFromDatabase()
        {
            // Arrange
            var product = new ProductEntity { Name = "Test Product", Price = 10.50m };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var inventoriesToAdd = new List<InventoryEntity>
            {
                new() { Quantity = 50, ProductId = product.Id },
                new() { Quantity = 75, ProductId = product.Id },
                new() { Quantity = 100, ProductId = product.Id }
            };

            foreach (var inventory in inventoriesToAdd)
            {
                await _repository.AddAsync(inventory);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAsync_ShouldReturnInventoryFromDatabase()
        {
            // Arrange
            var product = new ProductEntity { Name = "Test Product", Price = 10.50m };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var inventory = new InventoryEntity
            {
                Quantity = 100,
                ProductId = product.Id
            };

            await _repository.AddAsync(inventory);

            // Act
            var result = await _repository.GetAsync(i => i.Id == inventory.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(inventory.Id, result.Id);
            Assert.Equal(inventory.Quantity, result.Quantity);
            Assert.Equal(inventory.ProductId, result.ProductId);
        }
    }
}
