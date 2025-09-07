using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;

namespace RealEstate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly PropertyService _service;

    public PropertiesController(PropertyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? address,
                                           [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
    {
        var result = await _service.GetAllAsync(name, address, minPrice, maxPrice);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Property property)
    {
        var result = await _service.CreateAsync(property);
        return CreatedAtAction(nameof(GetById), new { id = property.IdProperty }, result);
    }
}
