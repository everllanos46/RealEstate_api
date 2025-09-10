using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.Services;

namespace RealEstate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyImagesController : ControllerBase
{
    private readonly PropertyImageService _service;

    public PropertyImagesController(PropertyImageService service)
    {
        _service = service;
    }

    [HttpPost("{propertyId}/upload")]
    public async Task<IActionResult> Upload(string propertyId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { Message = "No se subió ningún archivo." });

        var dto = new UploadFileDto
        {
            FileName = file.FileName,
            Content = file.OpenReadStream(),
            ContentType = file.ContentType
        };

        var response = await _service.UploadAsync(propertyId, dto);
        return StatusCode((int)response.HttpStatusCode, response);
    }
}
