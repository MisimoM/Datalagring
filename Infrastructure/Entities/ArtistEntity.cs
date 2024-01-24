using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    public class ArtistEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string ArtistName { get; set; } = null!;

        public virtual ICollection<AlbumEntity> Albums { get; set; } = new List<AlbumEntity>();
    }
}
