
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookRepository(DbContext dbContext) : BaseRepository<BookEntity>(dbContext)
    {
    }
}
