
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AuthorRepository(DbContext dbContext) : BaseRepository<AuthorEntity>(dbContext)
    {
    }
}
