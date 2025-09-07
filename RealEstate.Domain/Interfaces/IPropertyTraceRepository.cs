using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces;

public interface IPropertyTraceRepository
{
    Task<IEnumerable<PropertyTrace>> GetAllAsync();
    Task<PropertyTrace?> GetByIdAsync(string id);
    Task AddAsync(PropertyTrace propertyTrace);
    Task UpdateAsync(PropertyTrace propertyTrace);
    Task DeleteAsync(string id);
}