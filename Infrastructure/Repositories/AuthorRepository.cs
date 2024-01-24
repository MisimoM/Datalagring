using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories
{
    public class AuthorRepository(DataContext dbContext) : BaseRepository<AuthorEntity>(dbContext)
    {
    }
}
