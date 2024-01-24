
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GenreRepository(DataContext dbContext) : BaseRepository<GenreEntity>(dbContext)
    {
    }
}
