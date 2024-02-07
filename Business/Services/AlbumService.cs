using Infrastructure.Entities.Album;
using Infrastructure.Repositories.Album;
using Shared.Models.Album;
using System.Diagnostics;

namespace Business.Services
{
    public class AlbumService(AlbumRepository albumRepository, TrackService trackService, ArtistService artistService)
    {
        private readonly AlbumRepository _albumRepository = albumRepository;
        private readonly TrackService _trackService = trackService;
        private readonly ArtistService _artistService = artistService;

        public async Task<IEnumerable<AlbumModel>> GetAlbumsAsync()
        {
            var albumEntities = await _albumRepository.GetAllAsync();

            var albumModels = albumEntities.Select(albumEntity => new AlbumModel
            {
                Id = albumEntity.Id,
                Title = albumEntity.Title,
                Price = albumEntity.Price,
                Artist = albumEntity.Artist.Name,
                Tracks = albumEntity.Tracks.Select(trackEntity => new TrackModel
                {
                    Title = trackEntity.Title
                }).ToList()
            });

            return albumModels;
        }
        public async Task<AlbumEntity> CreateAlbumAsync(AlbumModel albumModel, List<TrackModel> trackModels)
        {
            try
            {
                var artistEntity = await _artistService.GetOrCreateArtistAsync(albumModel.Artist);

                var albumEntity = new AlbumEntity
                {
                    Title = albumModel.Title,
                    Price = albumModel.Price,
                    ArtistId = artistEntity.Id
                };

                var newAlbumEntity = await _albumRepository.AddAsync(albumEntity);

                if (newAlbumEntity != null)
                {
                    await _trackService.CreateTracksAsync(newAlbumEntity.Id, trackModels);

                    return newAlbumEntity;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return null!;
        }

        public async Task<bool> RemoveAlbumAsync(int albumId)
        {
            try
            {
                var albumEntity = await _albumRepository.GetAsync(album => album.Id == albumId);

                if (albumEntity != null)
                {
                    return await _albumRepository.DeleteAsync(album => album.Id == albumId);
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return false;
        }

        public async Task<AlbumEntity> UpdateAlbumAsync(int albumId, AlbumModel updatedAlbumModel, List<TrackModel> updatedTrackModels)
        {
            try
            {
                var existingAlbumEntity = await _albumRepository.GetAsync(album => album.Id == albumId);

                if (existingAlbumEntity != null)
                {
                    existingAlbumEntity.Title = updatedAlbumModel.Title;
                    existingAlbumEntity.Price = updatedAlbumModel.Price;

                    var artistEntity = await _artistService.GetOrCreateArtistAsync(updatedAlbumModel.Artist);
                    existingAlbumEntity.Artist = artistEntity;

                    var updatedAlbumEntity = await _albumRepository.UpdateAsync(album => album.Id == albumId, existingAlbumEntity);

                    await _trackService.UpdateTracksAsync(albumId, updatedTrackModels);

                    return updatedAlbumEntity;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return null!;
        }
    }
}
