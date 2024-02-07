using Infrastructure.Contexts;
using Infrastructure.Entities.Album;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Album
{
    public class TrackRepository(DataContext dbContext) : BaseRepository<TrackEntity, DataContext>(dbContext)
    {
        public async Task<IEnumerable<TrackEntity>> GetTracksByAlbumIdAsync(int albumId)
        {
            return await _dbContext.Tracks
                .Where(track => track.AlbumId == albumId)
                .ToListAsync();
        }
    }
}
