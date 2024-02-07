using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Album;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Album;

namespace Business.Tests.Services.Album
{
    public class TrackService_Tests
    {
        private readonly DataContext _context;
        private readonly TrackService _trackService;
        private readonly ArtistService _artistService;
        private readonly AlbumService _albumService;

        public TrackService_Tests()
        {
            _context = new(new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            var albumRepository = new AlbumRepository(_context);
            var artistRepository = new ArtistRepository(_context);
            var trackRepository = new TrackRepository(_context);

            _artistService = new ArtistService(artistRepository);
            _trackService = new TrackService(trackRepository);
            _albumService = new AlbumService(albumRepository, _trackService, _artistService);
        }

        [Fact]
        public async Task CreateTracksAsync_ShouldCreateNewTracksForAlbum()
        {
            // Arrange
            var artistEntity = await _artistService.GetOrCreateArtistAsync("Test Artist");

            var albumModel = new AlbumModel
            {
                Title = "Test Album",
                Price = 10.50m,
                Artist = artistEntity.Name
            };

            var trackModels = new List<TrackModel>
            {
                new() { Title = "Track 1" },
                new() { Title = "Track 2" },
                new() { Title = "Track 3" }
            };

            // Act
            var createdAlbum = await _albumService.CreateAlbumAsync(albumModel, trackModels);

            // Assert
            Assert.NotNull(createdAlbum);
            Assert.Equal(trackModels.Count, createdAlbum.Tracks.Count);
        }

        [Fact]
        public async Task UpdateTracksAsync_ShouldUpdateExistingTracksAndAddNewTracks()
        {
            // Arrange
            var artistEntity = await _artistService.GetOrCreateArtistAsync("Test Artist");

            var albumModel = new AlbumModel
            {
                Title = "Test Album",
                Price = 10.50m,
                Artist = artistEntity.Name
            };

            var trackModels = new List<TrackModel>
            {
                new() { Title = "Initial Track 1" },
                new() { Title = "Initial Track 2" },
                new() { Title = "Initial Track 3" }
            };

            var createdAlbum = await _albumService.CreateAlbumAsync(albumModel, trackModels);

            var updatedTrackModels = new List<TrackModel>
            {
                new() { Title = "Updated Track 1" },
                new() { Title = "Updated Track 2" }
            };

            // Act
            await _trackService.UpdateTracksAsync(createdAlbum.Id, updatedTrackModels);

            // Assert
            Assert.Equal(2, updatedTrackModels.Count);
            Assert.Equal("Updated Track 1", updatedTrackModels[0].Title);
            Assert.Equal("Updated Track 2", updatedTrackModels[1].Title);
        }
    }
}
