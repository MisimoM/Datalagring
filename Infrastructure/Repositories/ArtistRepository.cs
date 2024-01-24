using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ArtistRepository(DataContext dbContext) : BaseRepository<ArtistEntity>(dbContext)
    {
    }
}
