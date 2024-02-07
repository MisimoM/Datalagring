using Infrastructure.Contexts;
using Infrastructure.Entities.Album;
using Infrastructure.Repositories.Album;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.Album
{
    public class TrackRepository_Tests
    {
        private readonly DataContext _context;
        private readonly TrackRepository _repository;

        public TrackRepository_Tests()
        {
            _context = new DataContext(new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _repository = new TrackRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddTrackToDatabase()
        {
            // Arrange
            var album = new AlbumEntity { Title = "Test Album" };
            
            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();

            var track = new TrackEntity
            {
                Title = "Track Test",
                AlbumId = album.Id
            };

            // Act
            var result = await _repository.AddAsync(track);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(track.Id, result.Id);
            Assert.Equal(track.Title, result.Title);
            Assert.Equal(track.AlbumId, result.AlbumId);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteTrackFromDatabase()
        {
            // Arrange
            var album = new AlbumEntity { Title = "Test Album" };
            
            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();

            var track = new TrackEntity
            {
                Title = "Track Test",
                AlbumId = album.Id
            };

            await _repository.AddAsync(track);

            // Act
            var result = await _repository.DeleteAsync(t => t.Id == track.Id);

            // Assert
            Assert.True(result);
            var deletedTrack = await _repository.GetAsync(t => t.Id == track.Id);
            Assert.Null(deletedTrack);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTracksFromDatabase()
        {
            // Arrange
            var album = new AlbumEntity { Title = "Test Album" };
            
            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();

            var tracksToAdd = new List<TrackEntity>
            {
                new() { Title = "Track Test 1", AlbumId = album.Id },
                new() { Title = "Track Test 2", AlbumId = album.Id },
                new() { Title = "Track Test 3", AlbumId = album.Id }
            };

            foreach (var track in tracksToAdd)
            {
                await _repository.AddAsync(track);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAsync_ShouldReturnTrackFromDatabase()
        {
            // Arrange
            var album = new AlbumEntity { Title = "Test Album" };
            
            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();

            var track = new TrackEntity
            {
                Title = "Track Test",
                AlbumId = album.Id
            };

            await _repository.AddAsync(track);

            // Act
            var result = await _repository.GetAsync(t => t.Id == track.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(track.Id, result.Id);
            Assert.Equal(track.Title, result.Title);
            Assert.Equal(track.AlbumId, result.AlbumId);
        }

        [Fact]
        public async Task GetTracksByAlbumIdAsync_ShouldReturnTracksForGivenAlbumId()
        {
            // Arrange
            var album = new AlbumEntity { Title = "Test Album" };
            
            await _context.Albums.AddAsync(album);
            await _context.SaveChangesAsync();

            var albumId = album.Id;

            var tracksToAdd = new List<TrackEntity>
            {
                new() { Title = "Track Test 1", AlbumId = albumId },
                new() { Title = "Track Test 2", AlbumId = albumId },
                new() { Title = "Track Test 3", AlbumId = albumId }
            };

            foreach (var track in tracksToAdd)
            {
                await _repository.AddAsync(track);
            }

            // Act
            var result = await _repository.GetTracksByAlbumIdAsync(albumId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.All(result, track => Assert.Equal(albumId, track.AlbumId));
        }
    }
}
