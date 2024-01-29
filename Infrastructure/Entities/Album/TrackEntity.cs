using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities.Album
{
    public class TrackEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Required]
        public int AlbumId { get; set; }

        public virtual AlbumEntity Album { get; set; } = null!;
    }
}
