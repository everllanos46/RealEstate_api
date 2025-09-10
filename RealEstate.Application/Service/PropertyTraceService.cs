using AutoMapper;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using System.Net;

namespace RealEstate.Application.Services;


public class PropertyTraceService
{
    private readonly IPropertyTraceRepository _propertyRepository;
    private readonly IMapper _mapper;

    private const string ErrorMessage = "Error en el service de Property Trace";

    public PropertyTraceService(IPropertyTraceRepository repository, IMapper mapper)
    {
        _propertyRepository = repository;
        _mapper = mapper;
    }

    public async Task<Response<PropertyTraceDto>> CreateAsync(CreatePropertyTraceDto request)
    {
        try
        {
            var propertyTrace = _mapper.Map<PropertyTrace>(request);
            await _propertyRepository.AddAsync(propertyTrace);
            var propertyTraceDto = _mapper.Map<PropertyTraceDto>(propertyTrace);
            return new Response<PropertyTraceDto>("PropertyTrace registrado correctamente", HttpStatusCode.OK, propertyTraceDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response<PropertyTraceDto>(ErrorMessage, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<PropertyTraceDto>> GetByIdPropertyAsync(string propertyId)
    {
        try
        {
            var propertyTrace = await _propertyRepository.GetByIdPropertyAsync(propertyId);
            if (propertyTrace == null)
            {
                return new Response<PropertyTraceDto>(
                    $"No se encontró ningún PropertyTrace que tenga  '{propertyId}' como property",
                    HttpStatusCode.NotFound
                );
            }

            var propertyTraceDto = _mapper.Map<PropertyTraceDto>(propertyTrace);
            return new Response<PropertyTraceDto>(
                "Property Trace encontrado correctamente",
                HttpStatusCode.OK,
                propertyTraceDto
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response<PropertyTraceDto>(
                ErrorMessage,
                HttpStatusCode.InternalServerError
            );
        }
    }
}