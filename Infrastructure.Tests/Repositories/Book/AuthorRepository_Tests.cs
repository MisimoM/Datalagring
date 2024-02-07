using Infrastructure.Contexts;
using Infrastructure.Entities.Book;
using Infrastructure.Repositories.Book;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.Book
{
    public class AuthorRepository_Tests
    {
        private readonly DataContext _context;
        private readonly AuthorRepository _repository;

        public AuthorRepository_Tests()
        {
            _context = new DataContext(new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _repository = new AuthorRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddAuthorToDatabase()
        {
            // Arrange
            var author = new AuthorEntity
            { 
                Name = "Author Test"
            };

            // Act
            var result = await _repository.AddAsync(author);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(author.Id, result.Id);
            Assert.Equal(author.Name, result.Name);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAuthorFromDatabase()
        {
            // Arrange
            var author = new AuthorEntity
            {
                Name = "Author Test"
            };
            await _repository.AddAsync(author);

            // Act
            var result = await _repository.DeleteAsync(a => a.Id == author.Id);

            // Assert
            Assert.True(result);
            var deletedAuthor = await _repository.GetAsync(a => a.Id == author.Id);
            Assert.Null(deletedAuthor);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllAuthorsFromDatabase()
        {
            // Arrange
            var authorsToAdd = new List<AuthorEntity>
            {
                new() { Name = "Author Test 1" },
                new() { Name = "Author Test 2" },
                new() { Name = "Author Test 3" }
            };

            foreach (var author in authorsToAdd)
            {
                await _repository.AddAsync(author);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAsync_ShouldReturnAuthorFromDatabase()
        {
            // Arrange
            var author = new AuthorEntity
            {
                Name = "Author Test"
            };
            await _repository.AddAsync(author);

            // Act
            var result = await _repository.GetAsync(a => a.Id == author.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(author.Id, result.Id);
            Assert.Equal(author.Name, result.Name);
        }
    }
}
