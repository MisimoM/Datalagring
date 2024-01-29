using Infrastructure.Contexts;
using Infrastructure.Entities.Album;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Repositories.Album
{
    public class ArtistRepository(DataContext dbContext) : BaseRepository<ArtistEntity>(dbContext)
    {
    }
}
