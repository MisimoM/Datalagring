namespace Infrastructure.Entities.Product;

public partial class ManufacturerEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
}
