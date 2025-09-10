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
                var image = context.Items.ContainsKey("image") ? context.Items["image"] as PropertyImage : null;
                return image?.File ?? string.Empty;
            }))
            .ForMember(dest => dest.Owner, opt => opt.MapFrom((src, dest, destMember, context) =>
            {
                return context.Items.ContainsKey("owner") ? context.Items["owner"] as OwnerDto : null;
            }))
            .ForMember(dest => dest.PropertyTraceDto, opt => opt.MapFrom((src, dest, destMember, context) =>
            {
                return context.Items.ContainsKey("trace") ? context.Items["trace"] as PropertyTraceDto : null;
            }));

        CreateMap<CreatePropertyDto, Property>()
            .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.CodeInternal, opt => opt.MapFrom(src => $"INT-{DateTime.UtcNow.Ticks}"))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => DateTime.UtcNow.Year));
    }
}
