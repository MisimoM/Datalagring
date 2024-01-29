using Infrastructure.Entities.Album;
using Infrastructure.Entities.Book;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Context for Code first.
/// </summary>

namespace Infrastructure.Contexts
{
    public partial class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public virtual DbSet<BookEntity> Books { get; set; }
        public virtual DbSet<AuthorEntity> Authors { get; set; }
        public virtual DbSet<GenreEntity> Genres { get; set; }

        public virtual DbSet<AlbumEntity> Albums { get; set; }
        public virtual DbSet<ArtistEntity> Artists { get; set; }
        public virtual DbSet<TrackEntity> Tracks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlbumEntity>()
                .HasMany(al => al.Tracks)
                .WithOne(t => t.Album)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }


}
