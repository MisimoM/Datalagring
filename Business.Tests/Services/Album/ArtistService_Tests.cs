using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities.Album;
using Infrastructure.Repositories.Album;
using Microsoft.EntityFrameworkCore;

namespace Business.Tests.Services.Album
{
    public class ArtistService_Tests
    {
        private readonly DataContext _context;
        private readonly ArtistService _artistService;

        public ArtistService_Tests()
        {
            _context = new(new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);

            var artistRepository = new ArtistRepository(_context);
            _artistService = new ArtistService(artistRepository);
        }

        [Fact]
        public async Task GetOrCreateArtistAsync_ExistingArtist_ShouldReturnExistingArtist()
        {
            // Arrange
            var newArtist = new ArtistEntity { Name = "Test Artist" };
            await _artistService.GetOrCreateArtistAsync(newArtist.Name);


            // Act
            var existingArtist = new ArtistEntity { Name = "Test Artist" };
            var result = await _artistService.GetOrCreateArtistAsync(existingArtist.Name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newArtist.Name, result.Name);
        }

        [Fact]
        public async Task GetOrCreateArtistAsync_NewArtist_ShouldCreateAndReturnNewArtist()
        {
            // Arrange
            var newArtist = new ArtistEntity { Name = "Test Artist" };
            await _artistService.GetOrCreateArtistAsync(newArtist.Name);


            // Act
            var existingArtist = new ArtistEntity { Name = "Test Artist 2" };
            var result = await _artistService.GetOrCreateArtistAsync(existingArtist.Name);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(newArtist.Name, result.Name);
        }
    }
}
