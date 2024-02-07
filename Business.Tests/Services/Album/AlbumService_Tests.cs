using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Album;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Album;

namespace Business.Tests.Services.Album
{
    namespace Business.Tests.Services.Album
    {
        public class AlbumService_Tests
        {
            private readonly DataContext _context;
            private readonly AlbumService _albumService;
            private readonly ArtistService _artistService;
            private readonly TrackService _trackService;
            public AlbumService_Tests()
            {
                _context = new(new DbContextOptionsBuilder<DataContext>()
                    .UseInMemoryDatabase($"{System.Guid.NewGuid()}")
                    .Options);
                
                var artistRepository = new ArtistRepository(_context);
                var albumRepository = new AlbumRepository(_context);
                var trackRepository = new TrackRepository(_context);

                _artistService = new ArtistService(artistRepository);
                _trackService = new TrackService(trackRepository);
                _albumService = new AlbumService(albumRepository, _trackService, _artistService);
            }

            [Fact]
            public async Task GetAlbumsAsync_ShouldReturnAlbums()
            {
                // Arrange
                var artistEntity = await _artistService.GetOrCreateArtistAsync("Test Artist");

                var albumModels = new List<AlbumModel>
                {
                    new()
                    {
                        Title = "Test Album 1",
                        Price = 10.50m,
                        Artist = artistEntity.Name,
                        Tracks =
                        [
                            new() { Title = "Track 1" },
                            new() { Title = "Track 2" }
                        ]
                    },
                    new()
                    {
                        Title = "Test Album 2",
                        Price = 20.50m,
                        Artist = artistEntity.Name,
                        Tracks =
                        [
                            new TrackModel { Title = "Track 3" },
                            new TrackModel { Title = "Track 4" },
                        ]
                    }
                };

                foreach (var albumModel in albumModels)
                {
                    await _albumService.CreateAlbumAsync(albumModel, albumModel.Tracks);
                }

                // Act
                var result = await _albumService.GetAlbumsAsync();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
                Assert.Contains(result, album => album.Title == "Test Album 1");
                Assert.Contains(result, album => album.Title == "Test Album 2");
            }

            [Fact]
            public async Task CreateAlbumAsync_ShouldCreateNewAlbumWithTracks()
            {
                // Arrange
                var artistEntity = await _artistService.GetOrCreateArtistAsync("Test Artist");

                var albumModel = new AlbumModel
                {
                    Title = "Test Album",
                    Price = 10.50m,
                    Artist = artistEntity.Name,
                    Tracks =
                    [
                        new() { Title = "Track 1" },
                        new() { Title = "Track 2" },
                        new() { Title = "Track 3" },
                        new() { Title = "Track 4" }
                    ]
                };

                // Act
                var result = await _albumService.CreateAlbumAsync(albumModel, albumModel.Tracks);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(albumModel.Title, result.Title);
                Assert.Equal(albumModel.Tracks.Count, result.Tracks.Count);
            }

            [Fact]
            public async Task RemoveAlbumAsync_ShouldRemoveAlbum()
            {
                // Arrange
                var artistEntity = await _artistService.GetOrCreateArtistAsync("Test Artist");

                var albumModel = new AlbumModel
                {
                    Title = "Test Album",
                    Price = 10.50m,
                    Artist = artistEntity.Name,
                    Tracks =
                    [
                        new() { Title = "Track 1" },
                        new() { Title = "Track 2" }
                    ]
                };

                var createdAlbum = await _albumService.CreateAlbumAsync(albumModel, albumModel.Tracks);

                // Act
                var result = await _albumService.RemoveAlbumAsync(createdAlbum.Id);
                var removedAlbum = await _albumService.GetAlbumsAsync();

                // Assert
                Assert.True(result);
                Assert.Empty(removedAlbum);
            }

            [Fact]
            public async Task UpdateAlbumAsync_ShouldUpdateAlbumAndTracks()
            {
                // Arrange
                var artistEntity = await _artistService.GetOrCreateArtistAsync("Test Artist");

                var albumModel = new AlbumModel
                {
                    Title = "Test Album",
                    Price = 10.50m,
                    Artist = artistEntity.Name,
                    Tracks =
                    [
                        new() { Title = "Track 1" },
                        new() { Title = "Track 2" },
                    ]
                };

                var createdAlbum = await _albumService.CreateAlbumAsync(albumModel, albumModel.Tracks);

                // Act
                var UpdatedArtistEntity = await _artistService.GetOrCreateArtistAsync("Updated Artist");

                var updatedAlbumModel = new AlbumModel
                {
                    Title = "Updated Album",
                    Price = 20.50m,
                    Artist = UpdatedArtistEntity.Name,
                    Tracks =
                    [
                        new() { Title = "Updated Track 1" },
                        new() { Title = "Updated Track 2" },
                        new() { Title = "Updated Track 3" },
                        new() { Title = "Updated Track 4" }
                    ]
                };

                var result = await _albumService.UpdateAlbumAsync(createdAlbum.Id, updatedAlbumModel, updatedAlbumModel.Tracks);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(updatedAlbumModel.Title, result.Title);
                Assert.Equal(updatedAlbumModel.Price, result.Price);
                Assert.Equal(UpdatedArtistEntity.Name, result.Artist.Name);

                Assert.NotNull(result.Tracks);
                Assert.Equal(updatedAlbumModel.Tracks.Count, result.Tracks.Count);
            }
        }
    }
}
