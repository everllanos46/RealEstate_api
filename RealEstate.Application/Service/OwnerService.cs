using AutoMapper;
using RealEstate.Application.DTOs;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using System.Net;

namespace RealEstate.Application.Services;

public class OwnerService
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IFileStorageRepository _storageRepository;
    private readonly IMapper _mapper;
    private const string ErrorMessage = "Error en el service de owners";

    public OwnerService(IOwnerRepository repository, IFileStorageRepository storageRepository, IMapper mapper)
    {
        _ownerRepository = repository;
        _storageRepository = storageRepository;
        _mapper = mapper;
    }

    public async Task<Response<OwnerDto>> UploadAsync(string ownerId, UploadFileDto file)
    {
        try
        {
            if (file == null)
                return new Response<OwnerDto>("El archivo es obligatorio", HttpStatusCode.InternalServerError);
            var fileName = $"{ownerId}/{Guid.NewGuid()}_{file.FileName}";
            var url = await _storageRepository.UploadAsync(fileName, file.Content, file.ContentType);


            Owner updatedOwner = await _ownerRepository.UpdatePhotoAsync(ownerId, url);

            OwnerDto ownerDto = _mapper.Map<OwnerDto>(updatedOwner);

            return new Response<OwnerDto>("Imagen del owner " + ownerDto.Name + " actualizada correctamente", HttpStatusCode.OK, ownerDto);
        }
        catch (KeyNotFoundException ex)
        {
            return new Response<OwnerDto>(ex.Message, HttpStatusCode.NotFound);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new Response<OwnerDto>(ErrorMessage, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<OwnerDto>> CreateAsync(CreateOwnerDto request)
    {
        try
        {
            var owner = _mapper.Map<Owner>(request);

            await _ownerRepository.AddAsync(owner);

            var ownerDto = _mapper.Map<OwnerDto>(owner);

            return new Response<OwnerDto>("Owner registrado correctamente", HttpStatusCode.OK, ownerDto);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response<OwnerDto>(ErrorMessage, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Response<OwnerDto>> GetByIdAsync(string ownerId)
    {
        try
        {
            var owner = await _ownerRepository.GetByIdAsync(ownerId);

            if (owner == null)
            {
                return new Response<OwnerDto>(
                    $"No se encontró un Owner con Id '{ownerId}'",
                    HttpStatusCode.NotFound
                );
            }

            var ownerDto = _mapper.Map<OwnerDto>(owner);

            return new Response<OwnerDto>(
                "Owner encontrado correctamente",
                HttpStatusCode.OK,
                ownerDto
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new Response<OwnerDto>(
                ErrorMessage,
                HttpStatusCode.InternalServerError
            );
        }
    }



}