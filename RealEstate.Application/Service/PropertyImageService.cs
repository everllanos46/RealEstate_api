using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using System.Net;

namespace RealEstate.Application.Services;

public class PropertyImageService
{
    private readonly IPropertyImageRepository _repository;
    private readonly IFileStorageRepository _storageRepository;
    private const string ErrorMessage = "Error en el service de imágenes";

    public PropertyImageService(IPropertyImageRepository repository, IFileStorageRepository storageRepository)
    {
        _repository = repository;
        _storageRepository = storageRepository;
    }

    public async Task<Response<PropertyImage>> UploadAsync(string propertyId, UploadFileDto file)
    {
        try
        {
            if (file == null)
                return new Response<PropertyImage>("El archivo es obligatorio", HttpStatusCode.InternalServerError);
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

            return new Response<PropertyImage>("Imagen subida correctamente", HttpStatusCode.OK, image);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response<PropertyImage>(ErrorMessage, HttpStatusCode.InternalServerError);
        }
    }
}
