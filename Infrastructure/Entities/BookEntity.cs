using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class BookEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;
        
        [Required]
        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        [Required]
        [ForeignKey(nameof(AuthorEntity))]
        public int AuthorId { get; set; }
        public virtual AuthorEntity Author { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(GenreEntity))]
        public int GenreId { get; set; }

        public virtual GenreEntity Genre { get; set;} = null!;
    }
}
