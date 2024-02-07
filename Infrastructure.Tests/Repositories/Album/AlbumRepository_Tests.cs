using Infrastructure.Contexts;
using Infrastructure.Entities.Album;
using Infrastructure.Repositories.Album;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.Album
{
    public class AlbumRepository_Tests
    {
        private readonly DataContext _context;
        private readonly AlbumRepository _repository;

        public AlbumRepository_Tests()
        {
            _context = new DataContext(new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _repository = new AlbumRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddAlbumToDatabase()
        {
            // Arrange
            var artist = new ArtistEntity { Name = "Test Artist" };
            
            await _context.Artists.AddAsync(artist);
            await _context.SaveChangesAsync();

            var album = new AlbumEntity
            {
                Title = "Album Test",
                Price = 10.50m,
                ArtistId = artist.Id
            };

            // Act
            var result = await _repository.AddAsync(album);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(album.Id, result.Id);
            Assert.Equal(album.Title, result.Title);
            Assert.Equal(album.Price, result.Price);
            Assert.Equal(album.ArtistId, result.ArtistId);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAlbumFromDatabase()
        {
            // Arrange
            var artist = new ArtistEntity { Name = "Test Artist" };
            
            await _context.Artists.AddAsync(artist);
            await _context.SaveChangesAsync();

            var album = new AlbumEntity
            {
                Title = "Album Test",
                Price = 10.50m,
                ArtistId = artist.Id
            };

            await _repository.AddAsync(album);

            // Act
            var result = await _repository.DeleteAsync(a => a.Id == album.Id);

            // Assert
            Assert.True(result);
            var deletedAlbum = await _repository.GetAsync(a => a.Id == album.Id);
            Assert.Null(deletedAlbum);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAlbumsFromDatabase()
        {
            // Arrange
            var artist = new ArtistEntity { Name = "Test Artist" };
            
            await _context.Artists.AddAsync(artist);
            await _context.SaveChangesAsync();

            var albumsToAdd = new List<AlbumEntity>
            {
                new() { Title = "Album Test 1", Price = 10.50m, ArtistId = artist.Id },
                new() { Title = "Album Test 2", Price = 20.50m, ArtistId = artist.Id },
                new() { Title = "Album Test 3", Price = 30.50m, ArtistId = artist.Id }
            };

            foreach (var album in albumsToAdd)
            {
                await _repository.AddAsync(album);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAsync_ShouldReturnAlbumFromDatabase()
        {
            // Arrange
            var artist = new ArtistEntity { Name = "Test Artist" };
            
            await _context.Artists.AddAsync(artist);
            await _context.SaveChangesAsync();

            var album = new AlbumEntity
            {
                Title = "Album Test",
                Price = 10.50m,
                ArtistId = artist.Id
            };

            await _repository.AddAsync(album);

            // Act
            var result = await _repository.GetAsync(a => a.Id == album.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(album.Id, result.Id);
            Assert.Equal(album.Title, result.Title);
            Assert.Equal(album.Price, result.Price);
            Assert.Equal(album.ArtistId, result.ArtistId);
        }
    }
}
