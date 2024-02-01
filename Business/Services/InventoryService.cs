using Infrastructure.Repositories.Product;

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
    }
}
