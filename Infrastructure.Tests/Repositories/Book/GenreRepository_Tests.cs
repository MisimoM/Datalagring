using Infrastructure.Contexts;
using Infrastructure.Entities.Book;
using Infrastructure.Repositories.Book;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tests.Repositories.Book
{
    public class GenreRepository_Tests
    {
        private readonly DataContext _context;
        private readonly GenreRepository _repository;

        public GenreRepository_Tests()
        {
            _context = new DataContext(new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);
            _repository = new GenreRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddGenreToDatabase()
        {
            // Arrange
            var genre = new GenreEntity
            {
                Name = "Genre Test"
            };

            // Act
            var result = await _repository.AddAsync(genre);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(genre.Id, result.Id);
            Assert.Equal(genre.Name, result.Name);
            Assert.NotEqual(0, result.Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteGenreFromDatabase()
        {
            // Arrange
            var genre = new GenreEntity
            {
                Name = "Genre Test"
            };

            await _repository.AddAsync(genre);

            // Act
            var result = await _repository.DeleteAsync(g => g.Id == genre.Id);
            var deletedGenre = await _repository.GetAsync(g => g.Id == genre.Id);

            // Assert
            Assert.True(result);
            Assert.Null(deletedGenre);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllGenresFromDatabase()
        {
            // Arrange
            var genresToAdd = new List<GenreEntity>
            {
                new() { Name = "Genre Test 1" },
                new() { Name = "Genre Test 2" },
                new() { Name = "Genre Test 3" }
            };

            foreach (var genre in genresToAdd)
            {
                await _repository.AddAsync(genre);
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAsync_ShouldReturnGenreFromDatabase()
        {
            // Arrange
            var genre = new GenreEntity
            {
                Name = "Genre Test"
            };
            await _repository.AddAsync(genre);

            // Act
            var result = await _repository.GetAsync(g => g.Id == genre.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(genre.Id, result.Id);
            Assert.Equal(genre.Name, result.Name);
        }
    }
}
