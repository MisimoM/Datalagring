using Infrastructure.Entities.Album;
using Infrastructure.Repositories.Album;
using Shared.Models.Album;
using System.Diagnostics;

namespace Business.Services
{
    public class TrackService(TrackRepository trackRepository)
    {
        private readonly TrackRepository _trackRepository = trackRepository;
        public async Task CreateTracksAsync(int albumId, List<TrackModel> trackModels)
        {
            foreach (var trackModel in trackModels)
            {
                var trackEntity = new TrackEntity
                {
                    Title = trackModel.Title,
                    AlbumId = albumId
                };

                await _trackRepository.AddAsync(trackEntity);
            }
        }

        public async Task UpdateTracksAsync(int albumId, List<TrackModel> updatedTrackModels)
        {
            try
            {
                var existingTracks = await _trackRepository.GetTracksByAlbumIdAsync(albumId);

                foreach (var track in existingTracks)
                {
                    await _trackRepository.DeleteAsync(t => t.Id == track.Id);
                }

                foreach (var updatedTrackModel in updatedTrackModels)
                {
                    var existingTrack = existingTracks.FirstOrDefault(track => track.Title == updatedTrackModel.Title);

                    if (existingTrack != null)
                    {
                        existingTrack.Title = updatedTrackModel.Title;

                        await _trackRepository.UpdateAsync(track => track.Id == existingTrack.Id, existingTrack);
                    }
                    else
                    {
                        var newTrackEntity = new TrackEntity
                        {
                            Title = updatedTrackModel.Title,
                            AlbumId = albumId
                        };

                        await _trackRepository.AddAsync(newTrackEntity);
                    }
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
    }
}
