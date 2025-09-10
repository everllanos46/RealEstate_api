using AutoMapper;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using System.Net;

namespace RealEstate.Application.Services;

public class PropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IPropertyImageRepository _imageRepository;
    private readonly IMapper _mapper;
    private readonly OwnerService _ownerService;
    private readonly PropertyTraceService _propertyTraceService;

    public PropertyService(
        IPropertyRepository propertyRepository,
        IPropertyImageRepository imageRepository,
        IMapper mapper,
        OwnerService ownerService,
        PropertyTraceService propertyTraceService)
    {
        _propertyRepository = propertyRepository;
        _imageRepository = imageRepository;
        _mapper = mapper;
        _ownerService = ownerService;
        _propertyTraceService = propertyTraceService;
    }

    private const string ErrorMessage = "Error en el service de propiedades";

    public async Task<Response<PropertiesResponseDto>> GetAllAsync(
        string? name,
        string? address,
        decimal? minPrice,
        decimal? maxPrice,
        int pageNumber = 1,
        int pageSize = 10)
    {
        try
        {
            var (properties, totalCount) = await _propertyRepository.GetAllAsync(name, address, minPrice, maxPrice, pageNumber, pageSize);

            if (!properties.Any())
            {
                return new Response<PropertiesResponseDto>(
                    "No se encontraron propiedades",
                    HttpStatusCode.OK,
                    new PropertiesResponseDto
                    {
                        Properties = Enumerable.Empty<PropertyDto>(),
                        TotalCount = 0
                    }
                );
            }

            var propertyIds = properties.Select(p => p.IdProperty).ToList();
            var images = await _imageRepository.GetAllAsync(propertyIds);

            var propertyDtos = properties.Select(p =>
            {
                var image = images.FirstOrDefault(i => i.IdProperty == p.IdProperty);
                return _mapper.Map<PropertyDto>(p, opts => { opts.Items["image"] = image; });
            }).ToList();

            var responseDto = new PropertiesResponseDto
            {
                Properties = propertyDtos,
                TotalCount = totalCount
            };

            return new Response<PropertiesResponseDto>(
                "Propiedades consultadas correctamente",
                HttpStatusCode.OK,
                responseDto
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response<PropertiesResponseDto>(
                ErrorMessage,
                HttpStatusCode.InternalServerError,
                new PropertiesResponseDto
                {
                    Properties = Enumerable.Empty<PropertyDto>(),
                    TotalCount = 0
                }
            );
        }
    }

    public async Task<Response<PropertyDto>> GetByIdAsync(string idProperty)
    {
        try
        {
            Property? property = await _propertyRepository.GetByIdAsync(idProperty);
            if (property == null)
                return new Response<PropertyDto>("Propiedad no encontrada", HttpStatusCode.NoContent);

            var image = await _imageRepository.GetByIdAsync(property.IdProperty);

            var ownerResponse = await _ownerService.GetByIdAsync(property.IdOwner);
            OwnerDto? ownerDto = ownerResponse.Data;
            var traceResponse = await _propertyTraceService.GetByIdPropertyAsync(property.IdProperty);
            PropertyTraceDto? propertyTraceDto = traceResponse.Data;


            var propertyDto = _mapper.Map<PropertyDto>(property, opts =>
            {
                opts.Items["image"] = image;
                opts.Items["owner"] = ownerDto;
                opts.Items["trace"] = propertyTraceDto;
            });


            return new Response<PropertyDto>("Propiedad encontrada", HttpStatusCode.OK, propertyDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response<PropertyDto>(ErrorMessage, HttpStatusCode.InternalServerError);
        }
    }


    public async Task<Response<PropertyDto>> CreateAsync(CreatePropertyDto request)
    {
        try
        {
            var property = _mapper.Map<Property>(request);

            await _propertyRepository.AddAsync(property);

            var propertyDtoResponse = _mapper.Map<PropertyDto>(property);
            return new Response<PropertyDto>("Propiedad registrada", HttpStatusCode.OK, propertyDtoResponse);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response<PropertyDto>(ErrorMessage, HttpStatusCode.InternalServerError);
        }
    }
}
