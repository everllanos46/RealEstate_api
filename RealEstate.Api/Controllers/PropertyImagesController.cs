using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyImagesController : ControllerBase
{
    private readonly PropertyImageService _service;
    private readonly IPropertyImageRepository _repository;

    public PropertyImagesController(IPropertyImageRepository repository, PropertyImageService imageService)
    {
        _repository = repository;
        _service = imageService;
    }

    [HttpPost("{propertyId}/upload")]
    public async Task<IActionResult> Upload(string propertyId, IFormFile file)
{
    if (file == null || file.Length == 0)
        return BadRequest("No file uploaded.");

    var dto = new UploadFileDto
    {
        FileName = file.FileName,
        Content = file.OpenReadStream(),
        ContentType = file.ContentType
    };

    var image = await _service.UploadAsync(propertyId, dto);
    return Ok(image);
}

    [HttpGet("{propertyId}")]
    public async Task<IActionResult> GetByProperty(string propertyId)
    {
        var images = await _repository.GetByIdAsync(propertyId);
        return Ok(images);
    }
}
