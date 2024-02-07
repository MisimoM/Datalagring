using Business.Services;
using Infrastructure.Contexts;
using Infrastructure.Entities.Book;
using Infrastructure.Repositories.Book;
using Microsoft.EntityFrameworkCore;

namespace Business.Tests.Services.Book
{
    public class GenreService_Tests
    {
        private readonly DataContext _context;
        private readonly GenreService _genreService;

        public GenreService_Tests()
        {
            _context = new DataContext(new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"{Guid.NewGuid()}")
                .Options);

            var genreRepository = new GenreRepository(_context);
            _genreService = new GenreService(genreRepository);
        }

        [Fact]
        public async Task GetOrCreateGenreAsync_ExistingGenre_ShouldReturnExistingGenre()
        {
            // Arrange
            var newGenre = new GenreEntity { Name = " Test Genre" };
            await _genreService.GetOrCreateGenreAsync(newGenre.Name);

            // Act
            var existingGenre = new GenreEntity { Name = " Test Genre" };
            var result = await _genreService.GetOrCreateGenreAsync(existingGenre.Name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newGenre.Name, result.Name);
        }

        [Fact]
        public async Task GetOrCreateGenreAsync_NewGenre_ShouldCreateAndReturnNewGenre()
        {
            // Arrange
            var newGenre = new GenreEntity { Name = " Test Genre" };
            await _genreService.GetOrCreateGenreAsync(newGenre.Name);

            // Act
            var existingGenre = new GenreEntity { Name = " Test Genre 2" };
            var result = await _genreService.GetOrCreateGenreAsync(existingGenre.Name);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(newGenre.Name, result.Name);
        }
    }
}
