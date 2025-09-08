using RealEstate.Application.DTOs;
using RealEstate.Application.Mappers;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Services;

public class PropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IPropertyImageRepository _imageRepository;

    public PropertyService(IPropertyRepository propertyRepository, IPropertyImageRepository imageRepository)
    {
        _propertyRepository = propertyRepository;
        _imageRepository = imageRepository;
    }

    public async Task<(IEnumerable<PropertyDto> Properties, long TotalCount)> GetAllAsync(
    string? name,
    string? address,
    decimal? minPrice,
    decimal? maxPrice,
    int pageNumber = 1,
    int pageSize = 10)
    {
        var (properties, totalCount) = await _propertyRepository.GetAllAsync(name, address, minPrice, maxPrice, pageNumber, pageSize);

        var propertyIds = properties.Select(p => p.IdProperty).ToList();
        var images = await _imageRepository.GetAllAsync(propertyIds);

        var propertyDtos = properties.Select(p =>
        {
            var image = images.FirstOrDefault(i => i.IdProperty == p.IdProperty);
            return PropertyMapper.ToDto(p, image);
        }).ToList();

        return (propertyDtos, totalCount);
    }


    public async Task<PropertyDto?> GetByIdAsync(Property property)
    {
        if (property == null)
            return null;
        var image = await _imageRepository.GetByIdAsync(property.IdProperty);

        return PropertyMapper.ToDto(property, image);
    }


    public async Task<PropertyDto> CreateAsync(CreatePropertyDto request)
    {
        var property = new Property
        {
            IdProperty = Guid.NewGuid().ToString(),
            Name = request.Name,
            Address = request.Address,
            Price = request.Price,
            CodeInternal = $"INT-{DateTime.UtcNow.Ticks}",
            Year = DateTime.UtcNow.Year,
            IdOwner = request.IdOwner
        };

        await _propertyRepository.AddAsync(property);

        return await GetByIdAsync(property);
    }

}
