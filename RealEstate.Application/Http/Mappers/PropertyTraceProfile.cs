using AutoMapper;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Mappings
{
    public class PropertyTraceProfile : Profile
    {
        public PropertyTraceProfile()
        {
            CreateMap<PropertyTrace, PropertyTraceDto>();

            CreateMap<CreatePropertyTraceDto, PropertyTrace>()
                .ForMember(dest => dest.IdPropertyTrace, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
        }
    }
}
