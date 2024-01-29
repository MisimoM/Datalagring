namespace Infrastructure.Entities.Product;

public partial class InventoryEntity
{
    public int Id { get; set; }

    public int Quantity { get; set; }

    public int ProductId { get; set; }

    public virtual ProductEntity Product { get; set; } = null!;
}
