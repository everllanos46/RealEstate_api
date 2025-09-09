using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Services;

public class PropertyImageService
{
    private readonly IPropertyImageRepository _repository;
    private readonly IFileStorageRepository _storageRepository;

    public PropertyImageService(IPropertyImageRepository repository, IFileStorageRepository storageRepository)
    {
        _repository = repository;
        _storageRepository = storageRepository;
    }

    public async Task<PropertyImage> UploadAsync(string propertyId, UploadFileDto file)
    {
        if (file == null)
            throw new ArgumentNullException(nameof(file));

        if (string.IsNullOrEmpty(propertyId))
            throw new ArgumentNullException(nameof(propertyId));

        var fileName = $"{propertyId}/{Guid.NewGuid()}_{file.FileName}";
        var url = await _storageRepository.UploadAsync(fileName, file.Content, file.ContentType);

        var image = new PropertyImage
        {
            IdPropertyImage = Guid.NewGuid().ToString(),
            IdProperty = propertyId,
            File = url,
            Enabled = true
        };

        await _repository.AddAsync(image);
        return image;
    }

}
