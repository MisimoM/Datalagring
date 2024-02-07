using Infrastructure.Entities.Product;
using Infrastructure.Repositories.Product;

namespace Business.Services
{
    public class ManufacturerService(ManufacturerRepository manufacturerRepository)
    {
        private readonly ManufacturerRepository _manufacturerRepository =   manufacturerRepository;

        public async Task<ManufacturerEntity> GetOrCreateManufacturerAsync(string manufacturerName)
        {
            var existingManufacturer = await _manufacturerRepository.GetAsync(manufacturer => manufacturer.Name == manufacturerName);

            if (existingManufacturer != null)
            {
                return existingManufacturer;
            }

            var newManufacturer = await _manufacturerRepository.AddAsync(new ManufacturerEntity { Name = manufacturerName });
            return newManufacturer;
        }
    }
}
