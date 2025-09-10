using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.Services;

namespace RealEstate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OwnerController : ControllerBase
{
    private readonly OwnerService _service;

    public OwnerController(OwnerService service)
    {
        _service = service;
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOwnerDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _service.CreateAsync(request);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    [HttpPost("{ownerId}/upload")]
    public async Task<IActionResult> Upload(string ownerId, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { Message = "No se subió ningún archivo." });

        var dto = new UploadFileDto
        {
            FileName = file.FileName,
            Content = file.OpenReadStream(),
            ContentType = file.ContentType
        };

        var response = await _service.UploadAsync(ownerId, dto);
        return StatusCode((int)response.HttpStatusCode, response);
    }
}
