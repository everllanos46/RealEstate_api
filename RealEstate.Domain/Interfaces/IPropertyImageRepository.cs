using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces;

public interface IPropertyImageRepository
{
    Task<IEnumerable<PropertyImage>> GetAllAsync();
    Task<PropertyImage?> GetByIdAsync(string id);
    Task AddAsync(PropertyImage propertyImage);
}