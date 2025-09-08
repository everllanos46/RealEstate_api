using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Mappers;

public static class PropertyMapper
{
    public static PropertyDto ToDto(Property property, PropertyImage? image = null)
    {
        return new PropertyDto
        {
            IdOwner = property.IdOwner,
            Name = property.Name,
            Address = property.Address,
            Price = property.Price,
            ImageUrl = image?.File ?? string.Empty,
            IdProperty = property.IdProperty
        };
    }
}
