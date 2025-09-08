using RealEstate.Domain.Entities;

namespace RealEstate.Domain.Interfaces;

public interface IPropertyRepository
{
    Task<(IEnumerable<Property> Properties, long TotalCount)> GetAllAsync(
        string? name,
        string? address,
        decimal? minPrice,
        decimal? maxPrice,
        int pageNumber = 1,
        int pageSize = 10);
    Task<Property?> GetByIdAsync(string id);
    Task AddAsync(Property property);


}