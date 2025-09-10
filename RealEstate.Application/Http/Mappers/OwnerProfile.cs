using AutoMapper;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Mappings
{
    public class OwnerProfile : Profile
    {
        public OwnerProfile()
        {
            CreateMap<Owner, OwnerDto>();

            CreateMap<CreateOwnerDto, Owner>()
                .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
        }
    }
}
