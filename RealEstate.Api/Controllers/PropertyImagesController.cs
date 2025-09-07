using Microsoft.AspNetCore.Mvc;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyImagesController : ControllerBase
{
    private readonly IPropertyImageRepository _repository;

    public PropertyImagesController(IPropertyImageRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("{propertyId}/upload")]
    public async Task<IActionResult> Upload(string propertyId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "properties", propertyId);
        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);

        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(uploadsPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var image = new PropertyImage
        {
            IdProperty = propertyId,
            File = $"/uploads/properties/{propertyId}/{fileName}",
            Enabled = true
        };

        await _repository.AddAsync(image);
        return Ok(image);
    }

    [HttpGet("{propertyId}")]
    public async Task<IActionResult> GetByProperty(string propertyId)
    {
        var images = await _repository.GetByIdAsync(propertyId);
        return Ok(images);
    }
}
