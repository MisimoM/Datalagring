using Infrastructure.Contexts;
using Infrastructure.Entities.Album;

namespace Infrastructure.Repositories.Album
{
    public class ArtistRepository(DataContext dbContext) : BaseRepository<ArtistEntity, DataContext>(dbContext)
    {
    }
}
