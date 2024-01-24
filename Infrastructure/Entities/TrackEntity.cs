using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class TrackEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;
        
        [Required]
        [ForeignKey(nameof(AlbumEntity))]
        public int AlbumID { get; set; }

        public virtual AlbumEntity Album { get; set; } = new AlbumEntity();
    }
}
