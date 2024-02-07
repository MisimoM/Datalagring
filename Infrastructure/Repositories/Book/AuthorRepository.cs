using Infrastructure.Contexts;
using Infrastructure.Entities.Book;

namespace Infrastructure.Repositories.Book
{
    public class AuthorRepository(DataContext dbContext) : BaseRepository<AuthorEntity, DataContext>(dbContext)
    {
    }
}
