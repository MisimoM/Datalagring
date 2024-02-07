using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities.Book;
using Infrastructure.Repositories.Book;
using Microsoft.EntityFrameworkCore;

namespace Business.Tests.Services.Book
{
    public class AuthorService_Tests
    {

        private readonly DataContext _context;
        private readonly AuthorService _authorService;

        public AuthorService_Tests()
        {
            _context = new DataContext(new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            
            var authorRepository = new AuthorRepository(_context);
            _authorService = new AuthorService(authorRepository);
        }

        [Fact]
        public async Task GetOrCreateAuthorAsync_ExistingAuthor_ShouldReturnExistingAuthor()
        {
            // Arrange
            var newAuthor = new AuthorEntity { Name = " Test Author" };
            await _authorService.GetOrCreateAuthorAsync(newAuthor.Name);

            // Act
            var existingAuthor = new GenreEntity { Name = " Test Author" };
            var result = await _authorService.GetOrCreateAuthorAsync(existingAuthor.Name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newAuthor.Name, result.Name);
        }

        [Fact]
        public async Task GetOrCreateAuthorAsync_NewAuthor_ShouldCreateAndReturnNewAuthor()
        {
            // Arrange
            var newAuthor = new AuthorEntity { Name = " Test Author" };
            await _authorService.GetOrCreateAuthorAsync(newAuthor.Name);

            // Act
            var existingAuthor = new GenreEntity { Name = " Test Author 2" };
            var result = await _authorService.GetOrCreateAuthorAsync(existingAuthor.Name);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(newAuthor.Name, result.Name);
        }
    }
}
