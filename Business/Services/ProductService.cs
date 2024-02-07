using Infrastructure.Entities.Product;
using Infrastructure.Repositories.Product;
using Shared.Models.Product;
using System.Diagnostics;

namespace Business.Services
{
    public class ProductService(
        ProductRepository productRepository,
        CategoryService categoryService,
        ManufacturerService manufacturerService,
        InventoryService inventoryService)
    {
        private readonly ProductRepository _productRepository = productRepository;
        private readonly CategoryService _categoryService = categoryService;
        private readonly ManufacturerService _manufacturerService = manufacturerService;
        private readonly InventoryService _inventoryService = inventoryService;

        public async Task<IEnumerable<ProductModel>> GetProductsAsync()
        {
            var productEntities = await _productRepository.GetAllAsync();
            var productModels = new List<ProductModel>();

            foreach (var productEntity in productEntities)
            {
                var inventoryQuantity = await _inventoryService.GetInventoryQuantityAsync(productEntity.Id);

                var productModel = new ProductModel
                {
                    Id = productEntity.Id,
                    Name = productEntity.Name,
                    Price = productEntity.Price,
                    Manufacturer = productEntity.Manufacturer.Name,
                    Category = productEntity.Category.Name,
                    InventoryQuantity = inventoryQuantity
                };

                productModels.Add(productModel);
            }

            return productModels.AsEnumerable();
        }

        public async Task<ProductEntity> CreateProductAsync(ProductModel productModel)
        {
            try
            {
                var categoryEntity = await _categoryService.GetOrCreateCategoryAsync(productModel.Category);

                var manufacturerEntity = await _manufacturerService.GetOrCreateManufacturerAsync(productModel.Manufacturer);

                var productEntity = new ProductEntity
                {
                    Name = productModel.Name,
                    Price = productModel.Price,
                    CategoryId = categoryEntity.Id,
                    ManufacturerId = manufacturerEntity.Id,
                };

                var newProductEntity = await _productRepository.AddAsync(productEntity);

                await _inventoryService.CreateInventoryAsync(newProductEntity.Id, productModel.InventoryQuantity);

                if (newProductEntity != null)
                {
                    return newProductEntity;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return null!;
        }

        public async Task<bool> RemoveProductAsync(int productId)
        {
            try
            {
                var productEntity = await _productRepository.GetAsync(product => product.Id == productId);

                if (productEntity != null)
                {
                    return await _productRepository.DeleteAsync(product => product.Id == productId);
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return false;
        }

        public async Task<bool> UpdateProductAsync(int productId, ProductModel updatedProductModel)
        {
            try
            {
                var existingProductEntity = await _productRepository.GetAsync(product => product.Id == productId);

                if (existingProductEntity != null)
                {
                    existingProductEntity.Name = updatedProductModel.Name;
                    existingProductEntity.Price = updatedProductModel.Price;

                    var manufacturerEntity = await _manufacturerService.GetOrCreateManufacturerAsync(updatedProductModel.Manufacturer);
                    existingProductEntity.ManufacturerId = manufacturerEntity.Id;

                    var categoryEntity = await _categoryService.GetOrCreateCategoryAsync(updatedProductModel.Category);
                    existingProductEntity.CategoryId = categoryEntity.Id;

                    var updatedProduct = await _productRepository.UpdateAsync(product => product.Id == productId, existingProductEntity);

                    if (updatedProduct != null)
                    {
                        await _inventoryService.UpdateInventoryQuantityAsync(productId, updatedProductModel.InventoryQuantity);

                        return true;
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            
            return false;
        }
    }
}
