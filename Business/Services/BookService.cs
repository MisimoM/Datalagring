using Infrastructure.Entities.Book;
using Infrastructure.Repositories.Book;
using Shared.Models.Book;
using System.Diagnostics;

namespace Business.Services
{
    public class BookService(BookRepository bookRepository, AuthorService authorService, GenreService genreService)
    {
        private readonly BookRepository _bookRepository = bookRepository;
        private readonly AuthorService _authorService = authorService;
        private readonly GenreService _genreService = genreService;
        public async Task<IEnumerable<BookModel>> GetBooksAsync()
        {
            var bookEntities = await _bookRepository.GetAllAsync();

            var bookModels = bookEntities.Select(bookEntity => new BookModel
            {
                Id = bookEntity.Id,
                Title = bookEntity.Title,
                Price = bookEntity.Price,
                Author = bookEntity.Author.Name,
                Genre = bookEntity.Genre.Name
            });

            return bookModels;
        }

        public async Task<BookEntity> CreateBookAsync(BookModel bookModel)
        {
            try
            {
                var authorEntity = await _authorService.GetOrCreateAuthorAsync(bookModel.Author);

                var genreEntity = await _genreService.GetOrCreateGenreAsync(bookModel.Genre);

                var bookEntity = new BookEntity
                {
                    Title = bookModel.Title,
                    Price = bookModel.Price,
                    AuthorId = authorEntity.Id,
                    GenreId = genreEntity.Id
                };

                var newBookEntity = await _bookRepository.AddAsync(bookEntity);

                if (newBookEntity != null)
                {
                    return newBookEntity;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return null!;
        }

        public async Task<bool> RemoveBookAsync(int bookId)
        {
            try
            {
                var bookEntity = await _bookRepository.GetAsync(book => book.Id == bookId);

                if (bookEntity != null)
                {
                    return await _bookRepository.DeleteAsync(book => book.Id == bookId);
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
                
            return false;
        }

        public async Task<bool> UpdateBookAsync(int bookId, BookModel updatedBookModel)
        {
            try
            {
                var existingBookEntity = await _bookRepository.GetAsync(book => book.Id == bookId);

                if (existingBookEntity != null)
                {
                    existingBookEntity.Title = updatedBookModel.Title;
                    existingBookEntity.Price = updatedBookModel.Price;

                    var authorEntity = await _authorService.GetOrCreateAuthorAsync(updatedBookModel.Author);
                    existingBookEntity.AuthorId = authorEntity.Id;

                    var genreEntity = await _genreService.GetOrCreateGenreAsync(updatedBookModel.Genre);
                    existingBookEntity.GenreId = genreEntity.Id;

                    return await _bookRepository.UpdateAsync(book => book.Id == bookId, existingBookEntity) != null;
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return false;
        }
    }  
}
