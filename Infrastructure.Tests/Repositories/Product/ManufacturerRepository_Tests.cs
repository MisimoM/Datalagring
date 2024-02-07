using Infrastructure.Contexts;
using Infrastructure.Entities.Product;
using Infrastructure.Repositories.Product;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.Product
{
    public class ManufacturerRepository_Tests
    {
        private readonly ProductDbContext _context;
        private readonly ManufacturerRepository _repository;

        public ManufacturerRepository_Tests()
        {
            _context = new ProductDbContext(new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _repository = new ManufacturerRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddManufacturerToDatabase()
        {
            // Arrange
            var manufacturer = new ManufacturerEntity
            {
                Name = "Manufacturer Test"
            };

            // Act
            var result = await _repository.AddAsync(manufacturer);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(manufacturer.Id, result.Id);
            Assert.Equal(manufacturer.Name, result.Name);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteManufacturerFromDatabase()
        {
            // Arrange
            var manufacturer = new ManufacturerEntity
            {
                Name = "Manufacturer Test"
            };
            await _repository.AddAsync(manufacturer);

            // Act
            var result = await _repository.DeleteAsync(m => m.Id == manufacturer.Id);

            // Assert
            Assert.True(result);
            var deletedManufacturer = await _repository.GetAsync(m => m.Id == manufacturer.Id);
            Assert.Null(deletedManufacturer);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllManufacturersFromDatabase()
        {
            // Arrange
            var manufacturersToAdd = new List<ManufacturerEntity>
            {
                new() { Name = "Manufacturer Test 1" },
                new() { Name = "Manufacturer Test 2" },
                new() { Name = "Manufacturer Test 3" }
            };

            foreach (var manufacturer in manufacturersToAdd)
            {
                await _repository.AddAsync(manufacturer);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAsync_ShouldReturnManufacturerFromDatabase()
        {
            // Arrange
            var manufacturer = new ManufacturerEntity
            {
                Name = "Manufacturer Test"
            };
            await _repository.AddAsync(manufacturer);

            // Act
            var result = await _repository.GetAsync(m => m.Id == manufacturer.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(manufacturer.Id, result.Id);
            Assert.Equal(manufacturer.Name, result.Name);
        }
    }
}
