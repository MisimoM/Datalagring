
namespace Presentation.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string Author { get; set; } = null!;
        public string Genre { get; set; } = null!;
    }
}
