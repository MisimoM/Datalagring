using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AlbumRepository(DataContext dbContext) : BaseRepository<AlbumEntity>(dbContext)
    {
    }
}
