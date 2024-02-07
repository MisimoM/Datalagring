
using Infrastructure.Contexts;
using Infrastructure.Entities.Book;
using Infrastructure.Repositories.Book;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.Book
{
    public class BookRepository_Tests
    {
        private readonly DataContext _context;
        private readonly BookRepository _repository;

        public BookRepository_Tests()
        {
            _context = new DataContext(new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _repository = new BookRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddBookToDatabase()
        {
            // Arrange
            var author = new AuthorEntity { Name = "Test Author" };
            var genre = new GenreEntity { Name = "Test Genre" };
            
            await _context.Authors.AddAsync(author);
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();

            var book = new BookEntity
            {
                Title = "Book Test",
                Price = 10.50m,
                AuthorId = author.Id,
                GenreId = genre.Id
            };

            // Act
            var result = await _repository.AddAsync(book);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(book.Id, result.Id);
            Assert.Equal(book.Title, result.Title);
            Assert.Equal(book.Price, result.Price);
            Assert.Equal(book.AuthorId, result.AuthorId);
            Assert.Equal(book.GenreId, result.GenreId);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteBookFromDatabase()
        {
            // Arrange
            var author = new AuthorEntity { Name = "Test Author" };
            var genre = new GenreEntity { Name = "Test Genre" };
            
            await _context.Authors.AddAsync(author);
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();

            var book = new BookEntity
            {
                Title = "Book Test",
                Price = 10.50m,
                AuthorId = author.Id,
                GenreId = genre.Id
            };

            await _repository.AddAsync(book);

            // Act
            var result = await _repository.DeleteAsync(b => b.Id == book.Id);
            var deletedBook = await _repository.GetAsync(b => b.Id == book.Id);

            // Assert
            Assert.True(result);
            Assert.Null(deletedBook);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllBooksFromDatabase()
        {
            // Arrange
            var author = new AuthorEntity { Name = "Test Author" };
            var genre = new GenreEntity { Name = "Test Genre" };
            
            await _context.Authors.AddAsync(author);
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();

            var booksToAdd = new List<BookEntity>
            {
                new() { Title = "Book Test 1", Price = 10.50m, AuthorId = author.Id, GenreId = genre.Id },
                new() { Title = "Book Test 2", Price = 20.50m, AuthorId = author.Id, GenreId = genre.Id },
                new() { Title = "Book Test 3", Price = 30.50m, AuthorId = author.Id, GenreId = genre.Id }
            };

            foreach (var book in booksToAdd)
            {
                await _repository.AddAsync(book);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAsync_ShouldReturnBookFromDatabase()
        {
            // Arrange
            var author = new AuthorEntity { Name = "Test Author" };
            var genre = new GenreEntity { Name = "Test Genre" };
            
            await _context.Authors.AddAsync(author);
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();

            var book = new BookEntity
            {
                Title = "Book Test",
                Price = 10.50m,
                AuthorId = author.Id,
                GenreId = genre.Id
            };

            await _repository.AddAsync(book);

            // Act
            var result = await _repository.GetAsync(b => b.Id == book.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(book.Id, result.Id);
            Assert.Equal(book.Title, result.Title);
            Assert.Equal(book.Price, result.Price);
            Assert.Equal(book.AuthorId, result.AuthorId);
            Assert.Equal(book.GenreId, result.GenreId);
        }
    }
}
