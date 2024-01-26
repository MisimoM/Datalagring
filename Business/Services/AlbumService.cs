using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.Diagnostics;

namespace Business.Services
{
    public class AlbumService(AlbumRepository albumRepository, TrackRepository trackRepository, ArtistRepository artistRepository)
    {
        private readonly AlbumRepository _albumRepository = albumRepository;
        private readonly TrackRepository _trackRepository = trackRepository;
        private readonly ArtistRepository _artistRepository = artistRepository;

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
                var artistEntity = await _artistRepository.GetAsync(artist => artist.Name == albumModel.Artist) ??
                                   await _artistRepository.AddAsync(new ArtistEntity { Name = albumModel.Artist });

                var albumEntity = new AlbumEntity
                {
                    Title = albumModel.Title,
                    Price = albumModel.Price,
                    ArtistId = artistEntity.Id
                };

                var newAlbumEntity = await _albumRepository.AddAsync(albumEntity);

                if (newAlbumEntity != null)
                {
                    foreach (var trackModel in trackModels)
                    {
                        var trackEntity = new TrackEntity
                        {
                            Title = trackModel.Title,
                            AlbumId = newAlbumEntity.Id
                        };

                        await _trackRepository.AddAsync(trackEntity);
                    }

                    return newAlbumEntity;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

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
    }
}
