using Infrastructure.Entities.Product;
using Infrastructure.Repositories.Book;
using Infrastructure.Repositories.Product;
using Shared.Models.Product;
using System.Diagnostics;

namespace Business.Services
{
    public class ProductService(
        ProductRepository productRepository,
        CategoryRepository categoryRepository,
        ManufacturerRepository manufacturerRepository,
        InventoryRepository inventoryRepository)
    {
        private readonly ProductRepository _productRepository = productRepository;
        private readonly CategoryRepository _categoryRepository = categoryRepository;
        private readonly ManufacturerRepository _manufacturerRepository = manufacturerRepository;
        private readonly InventoryRepository _inventoryRepository = inventoryRepository;

        public async Task<IEnumerable<ProductModel>> GetProductsAsync()
        {
            var productEntites = await _productRepository.GetAllAsync();

            var productModels = productEntites.Select(productEntity => new ProductModel
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Price = productEntity.Price,
                Manufacturer = productEntity.Manufacturer.Name,
                Category = productEntity.Category.Name
            });

            return productModels;
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
                    ManufacturerId = manufacturerEntity.Id
                };

                var newProductEntity = await _productRepository.AddAsync(productEntity);

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

                    return await _productRepository.UpdateAsync(product => product.Id == productId, existingProductEntity) != null;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
            return false;
        }
    }

}
