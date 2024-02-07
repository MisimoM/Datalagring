using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities.Product;
using Infrastructure.Repositories.Product;
using Microsoft.EntityFrameworkCore;

namespace Business.Tests.Services.Product
{
    public class ManufacturerService_Tests
    {
        private readonly ProductDbContext _context;
        private readonly ManufacturerService _manufacturerService;

        public ManufacturerService_Tests()
        {
            _context = new(new DbContextOptionsBuilder<ProductDbContext>()
               .UseInMemoryDatabase($"{Guid.NewGuid()}")
               .Options);

            var manufacturerRepository = new ManufacturerRepository( _context );
            _manufacturerService = new ManufacturerService(manufacturerRepository);
        }

        [Fact]
        public async Task GetOrCreateManufacturerAsync_ExistingManufacturer_ShouldReturnExistingManufacturer()
        {
            // Arrange
            var newManufacturer = new ManufacturerEntity { Name = " Test Manufacturer" };
            await _manufacturerService.GetOrCreateManufacturerAsync(newManufacturer.Name);

            // Act
            var existingManufacturer = new ManufacturerEntity { Name = " Test Manufacturer" };
            var result = await _manufacturerService.GetOrCreateManufacturerAsync(existingManufacturer.Name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newManufacturer.Name, result.Name);
        }

        [Fact]
        public async Task GetOrCreateManufacturerAsync_NewManufacturer_ShouldCreateAndReturnNewManufacturer()
        {
            // Arrange
            var newManufacturer = new ManufacturerEntity { Name = " Test Manufacturer" };
            await _manufacturerService.GetOrCreateManufacturerAsync(newManufacturer.Name);

            // Act
            var existingManufacturer = new ManufacturerEntity { Name = " Test Manufacturer 2" };
            var result = await _manufacturerService.GetOrCreateManufacturerAsync(existingManufacturer.Name);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(newManufacturer.Name, result.Name);
        }
    }
}
