using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Product;
using Microsoft.EntityFrameworkCore;

namespace Business.Tests.Services.Product
{
    public class InventoryService_Tests
    {
        private readonly ProductDbContext _context;
        private readonly InventoryService _inventoryService;
        public InventoryService_Tests()
        {
            _context = new ProductDbContext(new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);

            var inventoryRepository = new InventoryRepository(_context);
            _inventoryService = new InventoryService(inventoryRepository);
        }

        [Fact]
        public async Task GetInventoryQuantityAsync_ShouldReturnZero_IfInventoryNotExists()
        {
            // Arrange
            var productId = 1;

            // Act
            var result = await _inventoryService.GetInventoryQuantityAsync(productId);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task CreateInventoryAsync_ShouldCreateInventoryAndReturnEntity()
        {
            // Arrange
            var productId = 1;
            var quantity = 10;

            // Act
            var result = await _inventoryService.CreateInventoryAsync(productId, quantity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.ProductId);
            Assert.Equal(quantity, result.Quantity);
        }

        [Fact]
        public async Task UpdateInventoryQuantityAsync_ShouldUpdateQuantity_IfInventoryExists()
        {
            // Arrange
            var productId = 3;
            var initialQuantity = 5;
            var updatedQuantity = 8;

            await _inventoryService.CreateInventoryAsync(productId, initialQuantity);

            // Act
            var updateResult = await _inventoryService.UpdateInventoryQuantityAsync(productId, updatedQuantity);
            var getResult = await _inventoryService.GetInventoryQuantityAsync(productId);

            // Assert
            Assert.True(updateResult);
            Assert.Equal(updatedQuantity, getResult);
        }

        [Fact]
        public async Task UpdateInventoryQuantityAsync_ShouldReturnFalse_IfInventoryNotExists()
        {
            // Arrange
            var productId = 4;
            var updatedQuantity = 8;

            // Act
            var result = await _inventoryService.UpdateInventoryQuantityAsync(productId, updatedQuantity);

            // Assert
            Assert.False(result);
        }
    }
}
