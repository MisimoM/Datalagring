using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Repositories
{
    public class ArtistRepository(DataContext dbContext) : BaseRepository<ArtistEntity>(dbContext)
    {
    }
}
