using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Repositories.Book;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Book;

namespace Business.Tests.Services.Book
{
    public class BookService_Tests
    {
        private readonly DataContext _context;
        private readonly BookService _bookService;
        private readonly AuthorService _authorService;
        private readonly GenreService _genreService;
        public BookService_Tests()
        {
            _context = new DataContext(new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);

            var bookRepository = new BookRepository(_context);
            var authorRepository = new AuthorRepository(_context);
            var genreRepository = new GenreRepository(_context);

            _authorService = new AuthorService(authorRepository);
            _genreService = new GenreService(genreRepository);
            _bookService = new BookService(bookRepository, _authorService, _genreService);
        }

        [Fact]
        public async Task GetBooksAsync_ShouldReturnBooks()
        {
            // Arrange
            var authorEntity = await _authorService.GetOrCreateAuthorAsync("Test Author");
            var genreEntity = await _genreService.GetOrCreateGenreAsync("Test Genre");

            var booksToAdd = new List<BookModel>
            {
                new() { Title = "Book Test 1", Price = 10.50m, Author = authorEntity.Name, Genre = genreEntity.Name},
                new() { Title = "Book Test 2", Price = 20.50m, Author = authorEntity.Name, Genre = genreEntity.Name },
                new() { Title = "Book Test 3", Price = 30.50m, Author = authorEntity.Name, Genre = genreEntity.Name }
            };

            foreach (var bookModel in booksToAdd)
            {
                await _bookService.CreateBookAsync(bookModel);
            }

            // Act
            var result = await _bookService.GetBooksAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task CreateBookAsync_ShouldCreateNewBook()
        {
            // Arrange
            var authorEntity = await _authorService.GetOrCreateAuthorAsync("Test Author");
            var genreEntity = await _genreService.GetOrCreateGenreAsync("Test Genre");

            var bookModel = new BookModel
            {
                Title = "Test Book",
                Price = 10.50m,
                Author = authorEntity.Name,
                Genre = genreEntity.Name
            };

            // Act
            var result = await _bookService.CreateBookAsync(bookModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(bookModel.Title, result.Title);
            Assert.Equal(bookModel.Price, result.Price);
        }

        [Fact]
        public async Task RemoveBookAsync_ShouldRemoveExistingBook()
        {
            // Arrange
            var authorEntity = await _authorService.GetOrCreateAuthorAsync("Test Author");
            var genreEntity = await _genreService.GetOrCreateGenreAsync("Test Genre");

            var bookModel = new BookModel
            {
                Title = "Test Book",
                Price = 10.50m,
                Author = authorEntity.Name,
                Genre = genreEntity.Name
            };

            var addedBook = await _bookService.CreateBookAsync(bookModel);

            // Act
            var result = await _bookService.RemoveBookAsync(addedBook.Id);
            var removedBook = await _bookService.GetBooksAsync();

            // Assert
            Assert.True(result);
            Assert.Empty(removedBook);
        }

        [Fact]
        public async Task UpdateBookAsync_ShouldUpdateExistingBook()
        {
            // Arrange
            var authorEntity = await _authorService.GetOrCreateAuthorAsync("Test Author");
            var genreEntity = await _genreService.GetOrCreateGenreAsync("Test Genre");

            var bookModel = new BookModel
            {
                Title = "Test Book",
                Price = 10.50m,
                Author = authorEntity.Name,
                Genre = genreEntity.Name
            };

            var createdBook = await _bookService.CreateBookAsync(bookModel);

            // Act
            var updatedAuthorEntity = await _authorService.GetOrCreateAuthorAsync("Updated Author");
            var updatedGenreEntity = await _genreService.GetOrCreateGenreAsync("Updated Genre");

            var updatedBookModel = new BookModel
            {
                Title = "Updated Test Book",
                Price = 20.50m,
                Author = updatedAuthorEntity.Name,
                Genre = updatedGenreEntity.Name
            };

            var result = await _bookService.UpdateBookAsync(createdBook.Id, updatedBookModel);

            // Assert
            Assert.True(result);
            Assert.Equal(updatedBookModel.Title, createdBook.Title);
            Assert.Equal(updatedBookModel.Price, createdBook.Price);
            Assert.Equal(updatedBookModel.Author, createdBook.Author.Name);
            Assert.Equal(updatedBookModel.Genre, createdBook.Genre.Name);
        }
    }
}
