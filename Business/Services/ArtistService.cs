using Infrastructure.Entities.Album;
using Infrastructure.Repositories.Album;

namespace Business.Services
{
    public class ArtistService(ArtistRepository artistRepository)
    {
        private readonly ArtistRepository _artistRepository = artistRepository;

        public async Task<ArtistEntity> GetOrCreateArtistAsync(string artistName)
        {
            var existingArtist = await _artistRepository.GetAsync(artist => artist.Name == artistName);

            if (existingArtist != null)
            {
                return existingArtist;
            }

            var newArtist = await _artistRepository.AddAsync(new ArtistEntity { Name = artistName });
            return newArtist;
        }
    }
}
