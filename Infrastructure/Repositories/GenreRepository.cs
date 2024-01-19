
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GenreRepository(DbContext dbContext) : BaseRepository<GenreEntity>(dbContext)
    {
    }
}
