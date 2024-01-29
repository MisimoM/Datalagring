namespace Infrastructure.Entities.Product;

public partial class ProductEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int ManufacturerId { get; set; }

    public int CategoryId { get; set; }

    public virtual CategoryEntity Category { get; set; } = null!;

    public virtual ICollection<InventoryEntity> Inventories { get; set; } = new List<InventoryEntity>();

    public virtual ManufacturerEntity Manufacturer { get; set; } = null!;
}
