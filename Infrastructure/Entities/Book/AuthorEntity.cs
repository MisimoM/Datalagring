using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities.Book
{
    [Index(nameof(Name), IsUnique = true)]
    public class AuthorEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        public virtual ICollection<BookEntity> Books { get; set; } = new List<BookEntity>();
    }
}
