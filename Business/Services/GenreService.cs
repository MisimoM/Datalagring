using Infrastructure.Entities.Book;
using Infrastructure.Repositories.Book;

namespace Business.Services
{
    public class GenreService(GenreRepository genreRepository)
    {
        private readonly GenreRepository _genreRepository = genreRepository;

        public async Task<GenreEntity> GetOrCreateGenreAsync(string genreName)
        {
            var existingGenre = await _genreRepository.GetAsync(genre => genre.Name == genreName);

            if (existingGenre != null)
            {
                return existingGenre;
            }

            var newGenre = await _genreRepository.AddAsync(new GenreEntity { Name = genreName });
            return newGenre;
        }
    }
}
