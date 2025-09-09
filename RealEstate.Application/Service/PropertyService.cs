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

    public PropertyService(
        IPropertyRepository propertyRepository,
        IPropertyImageRepository imageRepository,
        IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _imageRepository = imageRepository;
        _mapper = mapper;
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
                    HttpStatusCode.NotFound,
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

    public async Task<Response<PropertyDto>> GetByIdAsync(Property property)
    {
        try
        {
            if (property == null)
                return new Response<PropertyDto>("Propiedad no encontrada", HttpStatusCode.NotFound);

            var image = await _imageRepository.GetByIdAsync(property.IdProperty);
            var propertyDto = _mapper.Map<PropertyDto>(property, opts => { opts.Items["image"] = image; });

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

            var propertyDtoResponse = await GetByIdAsync(property);
            return propertyDtoResponse;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response<PropertyDto>(ErrorMessage, HttpStatusCode.InternalServerError);
        }
    }
}
