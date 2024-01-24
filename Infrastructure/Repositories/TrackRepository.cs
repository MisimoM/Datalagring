using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TrackRepository(DataContext dbContext) : BaseRepository<TrackEntity>(dbContext)
    {
    }
}
