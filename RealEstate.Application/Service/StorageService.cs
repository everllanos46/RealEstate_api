using RealEstate.Application.DTOs;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Services;

public class FileService : IFileService
{
    private readonly IFileStorageRepository _storageRepository;

    public FileService(IFileStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;
    }

    public async Task<string> UploadFileAsync(string id, UploadFileDto file)
    {
        if (file == null)
            throw new ArgumentNullException(nameof(file), "El archivo no puede ser nulo.");

        if (string.IsNullOrEmpty(id))
            throw new ArgumentException("El folder no puede ser vacío", nameof(id));

        var fileName = $"{id}/{Guid.NewGuid()}_{file.FileName}";
        var url = await _storageRepository.UploadAsync(fileName, file.Content, file.ContentType);

        return url;
    }
}
