using Infrastructure.Contexts;
using Infrastructure.Entities.Album;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Repositories.Album
{
    public class AlbumRepository(DataContext dbContext) : BaseRepository<AlbumEntity>(dbContext)
    {
        public override async Task<IEnumerable<AlbumEntity>> GetAllAsync()
        {
            try
            {
                var entities = await _dbContext.Set<AlbumEntity>()
                    .Include(album => album.Artist)
                    .Include(album => album.Tracks)
                    .ToListAsync();

                return entities;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Enumerable.Empty<AlbumEntity>();
            }
        }


    }
}
