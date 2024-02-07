using Infrastructure.Entities.Book;
using Infrastructure.Repositories.Book;

namespace Business.Services
{
    public class AuthorService(AuthorRepository authorRepository)
    {
        private readonly AuthorRepository _authorRepository = authorRepository;

        public async Task<AuthorEntity> GetOrCreateAuthorAsync(string authorName)
        {
            var existingAuthor = await _authorRepository.GetAsync(author => author.Name == authorName);

            if (existingAuthor != null)
            {
                return existingAuthor;
            }

            var newAuthor = await _authorRepository.AddAsync(new AuthorEntity { Name = authorName });
            return newAuthor;
        }
    }
}
