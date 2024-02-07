using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Product;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Product;

namespace Business.Tests.Services.Product
{
    public class ProductService_Tests
    {
        private readonly ProductDbContext _context;
        private readonly ProductService _productService;

        private readonly CategoryService _categoryService;
        private readonly ManufacturerService _manufacturerService;
        private readonly InventoryService _inventoryService;

        public ProductService_Tests()
        {
            _context = new ProductDbContext(new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);

            var productRepository = new ProductRepository(_context);
            var inventoryRepository = new InventoryRepository(_context);

            var categoryRepository = new CategoryRepository(_context);
            var manufacturerRepository = new ManufacturerRepository(_context);

            _categoryService = new CategoryService(categoryRepository);
            _manufacturerService = new ManufacturerService(manufacturerRepository);
            _inventoryService = new InventoryService(inventoryRepository);

            _productService = new ProductService(productRepository, _categoryService, _manufacturerService, _inventoryService);
        }

        [Fact]
        public async Task GetProductsAsync_ShouldReturnProducts()
        {
            // Arrange
            var categoryEntity = await _categoryService.GetOrCreateCategoryAsync("Test Category");
            var manufacturerEntity = await _manufacturerService.GetOrCreateManufacturerAsync("Test Manufacturer");
            
                var productsToAdd = new List<ProductModel>
                {
                    new()
                    {
                        Name = "Product 1",
                        Price = 10.50m,
                        Category = categoryEntity.Name,
                        Manufacturer = manufacturerEntity.Name,
                        InventoryQuantity = 3
                    },
                    new()
                    {
                        Name = "Product 2",
                        Price = 20.50m,
                        Category = categoryEntity.Name,
                        Manufacturer = manufacturerEntity.Name,
                        InventoryQuantity = 5
                    },
                    new()
                    {
                        Name = "Product 3",
                        Price = 30.50m,
                        Category = categoryEntity.Name,
                        Manufacturer = manufacturerEntity.Name,
                        InventoryQuantity = 8
                    }
                };

            foreach (var productModel in productsToAdd)
            {
                await _inventoryService.CreateInventoryAsync(productModel.Id, productModel.InventoryQuantity);
                await _productService.CreateProductAsync(productModel);
            }

            // Act
            var result = await _productService.GetProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task CreateProductAsync_ShouldCreateProduct()
        {
            // Arrange
            var categoryEntity = await _categoryService.GetOrCreateCategoryAsync("Test Category");
            var manufacturerEntity = await _manufacturerService.GetOrCreateManufacturerAsync("Test Manufacturer");

            var productModel = new ProductModel
            {
                Name = "Test Product",
                Price = 10.50m,
                Category = categoryEntity.Name,
                Manufacturer = manufacturerEntity.Name,
                InventoryQuantity = 5
            };

            await _inventoryService.CreateInventoryAsync(productModel.Id, productModel.InventoryQuantity);
            var createdProduct = await _productService.CreateProductAsync(productModel);
            var inventoryQuantity = await _inventoryService.GetInventoryQuantityAsync(createdProduct.Id);

            // Assert
            Assert.NotNull(createdProduct);
            Assert.Equal(productModel.Name, createdProduct.Name);
            Assert.Equal(productModel.Price, createdProduct.Price);
            Assert.Equal(productModel.Category, createdProduct.Category.Name);
            Assert.Equal(productModel.Manufacturer, createdProduct.Manufacturer.Name);
            Assert.Equal(productModel.InventoryQuantity, inventoryQuantity);
        }

        [Fact]
        public async Task RemoveProductAsync_ShouldRemoveProduct()
        {
            // Arrange
            var categoryEntity = await _categoryService.GetOrCreateCategoryAsync("Test Category");
            var manufacturerEntity = await _manufacturerService.GetOrCreateManufacturerAsync("Test Manufacturer");

            var productModel = new ProductModel
            {
                Name = "Test Product",
                Price = 10.50m,
                Category = categoryEntity.Name,
                Manufacturer = manufacturerEntity.Name,
                InventoryQuantity = 5
            };

            await _inventoryService.CreateInventoryAsync(productModel.Id, productModel.InventoryQuantity);
            var createdProduct = await _productService.CreateProductAsync(productModel);

            // Act
            var result = await _productService.RemoveProductAsync(createdProduct.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProduct()
        {
            // Arrange
            var categoryEntity = await _categoryService.GetOrCreateCategoryAsync("Test Category");
            var manufacturerEntity = await _manufacturerService.GetOrCreateManufacturerAsync("Test Manufacturer");

            var productModel = new ProductModel
            {
                Name = "Test Product",
                Price = 10.50m,
                Category = categoryEntity.Name,
                Manufacturer = manufacturerEntity.Name,
                InventoryQuantity = 5
            };
            
            await _inventoryService.CreateInventoryAsync(productModel.Id, productModel.InventoryQuantity);
            var createdProduct = await _productService.CreateProductAsync(productModel);

            var updatedCategoryEntity = await _categoryService.GetOrCreateCategoryAsync("Updated Category");
            var updatedManufacturerEntity = await _manufacturerService.GetOrCreateManufacturerAsync("Updated Manufacturer");

            var updatedProductModel = new ProductModel
            {
                Name = "Updated Product",
                Price = 20.50m,
                Category = updatedCategoryEntity.Name,
                Manufacturer = updatedManufacturerEntity.Name,
                InventoryQuantity = 10
            };

            // Act
            await _inventoryService.UpdateInventoryQuantityAsync(updatedProductModel.Id, updatedProductModel.InventoryQuantity);
            var result = await _productService.UpdateProductAsync(createdProduct.Id, updatedProductModel);
            var updatedInventoryQuantity = await _inventoryService.GetInventoryQuantityAsync(createdProduct.Id);

            // Assert
            Assert.True(result);
            Assert.Equal(updatedProductModel.Name, createdProduct.Name);
            Assert.Equal(updatedProductModel.Price, createdProduct.Price);
            Assert.Equal(updatedProductModel.Category, createdProduct.Category.Name);
            Assert.Equal(updatedProductModel.Manufacturer, createdProduct.Manufacturer.Name);
            Assert.Equal(updatedProductModel.InventoryQuantity, updatedInventoryQuantity);
        }
    }
}
