using Infrastructure.Contexts;
using Infrastructure.Entities.Book;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Infrastructure.Repositories.Book
{
    public class BookRepository(DataContext dbContext) : BaseRepository<BookEntity, DataContext>(dbContext)
    {
        public override async Task<IEnumerable<BookEntity>> GetAllAsync()
        {
            try
            {
                var entities = await _dbContext.Set<BookEntity>()
                    .Include(book => book.Author)
                    .Include(book => book.Genre)
                    .ToListAsync();

                return entities;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return Enumerable.Empty<BookEntity>();
            }
        }
    }
}
