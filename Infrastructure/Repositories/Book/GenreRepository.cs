using Infrastructure.Contexts;
using Infrastructure.Entities.Book;

namespace Infrastructure.Repositories.Book
{
    public class GenreRepository(DataContext dbContext) : BaseRepository<GenreEntity, DataContext>(dbContext)
    {
    }
}
