using Infrastructure.Contexts;
using Infrastructure.Entities.Album;
using Infrastructure.Repositories.Album;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.Album
{
    public class ArtistRepository_Tests
    {
        private readonly DataContext _context;
        private readonly ArtistRepository _repository;

        public ArtistRepository_Tests()
        {
            _context = new DataContext(new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _repository = new ArtistRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddArtistToDatabase()
        {
            // Arrange
            var artist = new ArtistEntity
            {
                Name = "Artist Test"
            };

            // Act
            var result = await _repository.AddAsync(artist);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(artist.Id, result.Id);
            Assert.Equal(artist.Name, result.Name);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteArtistFromDatabase()
        {
            // Arrange
            var artist = new ArtistEntity
            {
                Name = "Artist Test"
            };

            await _repository.AddAsync(artist);

            // Act
            var result = await _repository.DeleteAsync(a => a.Id == artist.Id);

            // Assert
            Assert.True(result);
            var deletedArtist = await _repository.GetAsync(a => a.Id == artist.Id);
            Assert.Null(deletedArtist);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllArtistsFromDatabase()
        {
            // Arrange
            var artistsToAdd = new List<ArtistEntity>
            {
                new() { Name = "Artist Test 1" },
                new() { Name = "Artist Test 2" },
                new() { Name = "Artist Test 3" }
            };

            foreach (var artist in artistsToAdd)
            {
                await _repository.AddAsync(artist);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAsync_ShouldReturnArtistFromDatabase()
        {
            // Arrange
            var artist = new ArtistEntity
            {
                Name = "Artist Test"
            };

            await _repository.AddAsync(artist);

            // Act
            var result = await _repository.GetAsync(a => a.Id == artist.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(artist.Id, result.Id);
            Assert.Equal(artist.Name, result.Name);
        }
    }
}
