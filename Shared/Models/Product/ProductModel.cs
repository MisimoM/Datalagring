namespace Shared.Models.Product
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Manufacturer { get; set; } = null!;
        public string Category { get; set; } = null!;


    }
}
