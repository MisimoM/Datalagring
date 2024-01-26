using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class AlbumEntity
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
        public int ArtistId { get; set; }
        public virtual ArtistEntity Artist { get; set; } = null!;
        public virtual ICollection<TrackEntity> Tracks { get; set; } = new List<TrackEntity>();
    }
}
