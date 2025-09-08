namespace RealEstate.Domain.Interfaces;

public interface IFileStorageRepository
{
    Task<string> UploadAsync(string path, Stream content, string contentType);
    Task DeleteAsync(string path);
}
