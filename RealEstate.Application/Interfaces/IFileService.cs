using RealEstate.Application.DTOs;

namespace RealEstate.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(string propertyId, UploadFileDto file);
    }
}