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
        [ForeignKey(nameof(ArtistEntity))]
        public int ArtistId { get; set; }


        public virtual ICollection<TrackEntity> Tracks { get; set; } = new List<TrackEntity>();

    }
}
