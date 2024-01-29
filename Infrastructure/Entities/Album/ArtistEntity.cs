using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities.Album
{
    [Index(nameof(Name), IsUnique = true)]
    public class ArtistEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        public virtual ICollection<AlbumEntity> Albums { get; set; } = new List<AlbumEntity>();
    }
}
