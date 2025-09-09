using AutoMapper;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;

public class PropertyProfile : Profile
{
    public PropertyProfile()
    {
        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom((src, dest, destMember, context) =>
            {
                var image = context.Items["image"] as PropertyImage;
                return image?.File ?? string.Empty;
            }));
    }
}
