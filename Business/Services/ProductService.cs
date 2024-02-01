using Infrastructure.Entities.Product;
using Infrastructure.Repositories.Product;
using Shared.Models.Product;
using System.Diagnostics;

namespace Business.Services
{
    public class ProductService(
        ProductRepository productRepository,
        CategoryRepository categoryRepository,
        ManufacturerRepository manufacturerRepository,
        InventoryRepository inventoryRepository,
        InventoryService inventoryService)
    {
        private readonly ProductRepository _productRepository = productRepository;
        private readonly CategoryRepository _categoryRepository = categoryRepository;
        private readonly ManufacturerRepository _manufacturerRepository = manufacturerRepository;
        private readonly InventoryRepository _inventoryRepository = inventoryRepository;
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
                var categoryEntity = await _categoryRepository.GetAsync(category => category.Name == productModel.Category) ??
                           await _categoryRepository.AddAsync(new CategoryEntity { Name = productModel.Category });

                var manufacturerEntity = await _manufacturerRepository.GetAsync(manufacturer => manufacturer.Name == productModel.Manufacturer) ??
                                  await _manufacturerRepository.AddAsync(new ManufacturerEntity { Name = productModel.Manufacturer });

                var productEntity = new ProductEntity
                {
                    Name = productModel.Name,
                    Price = productModel.Price,
                    CategoryId = categoryEntity.Id,
                    ManufacturerId = manufacturerEntity.Id,
                };

                var newProductEntity = await _productRepository.AddAsync(productEntity);

                var inventoryEntity = new InventoryEntity
                {
                    Quantity = productModel.InventoryQuantity,
                    ProductId = newProductEntity.Id
                };

                await _inventoryRepository.AddAsync(inventoryEntity);

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

                    var manufacturerEntity = await _manufacturerRepository.GetAsync(manufacturer => manufacturer.Name == updatedProductModel.Manufacturer) ??
                                       await _manufacturerRepository.AddAsync(new ManufacturerEntity { Name = updatedProductModel.Manufacturer });
                    existingProductEntity.ManufacturerId = manufacturerEntity.Id;

                    var categoryEntity = await _categoryRepository.GetAsync(category => category.Name == updatedProductModel.Category) ??
                                      await _categoryRepository.AddAsync(new CategoryEntity { Name = updatedProductModel.Category });
                    existingProductEntity.CategoryId = categoryEntity.Id;

                    var updatedProduct = await _productRepository.UpdateAsync(product => product.Id == productId, existingProductEntity);

                    if (updatedProduct != null)
                    {
                        var inventoryEntity = await _inventoryRepository.GetAsync(inventory => inventory.ProductId == productId);
                        if (inventoryEntity != null)
                        {
                            inventoryEntity.Quantity = updatedProductModel.InventoryQuantity;
                            await _inventoryRepository.UpdateAsync(inventory => inventory.ProductId == productId, inventoryEntity);
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            
            return false;
        }
    }
}
