using Infrastructure.Entities.Product;
using Infrastructure.Repositories.Product;
using System.Diagnostics;

namespace Business.Services
{
    public class InventoryService(InventoryRepository inventoryRepository)
    {
        private readonly InventoryRepository _inventoryRepository = inventoryRepository;

        public async Task<int> GetInventoryQuantityAsync(int productId)
        {
            var inventoryEntity = await _inventoryRepository.GetAsync(inventory => inventory.ProductId == productId);
            return inventoryEntity?.Quantity ?? 0;
        }

        public async Task<InventoryEntity> CreateInventoryAsync(int productId, int quantity)
        {
            var inventoryEntity = new InventoryEntity
            {
                Quantity = quantity,
                ProductId = productId
            };

            return await _inventoryRepository.AddAsync(inventoryEntity);
        }

        public async Task<bool> UpdateInventoryQuantityAsync(int productId, int quantity)
        {
            try
            {
                var inventoryEntity = await _inventoryRepository.GetAsync(inventory => inventory.ProductId == productId);

                if (inventoryEntity != null)
                {
                    inventoryEntity.Quantity = quantity;
                    await _inventoryRepository.UpdateAsync(inventory => inventory.ProductId == productId, inventoryEntity);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
