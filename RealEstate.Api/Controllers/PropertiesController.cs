using Microsoft.AspNetCore.Mvc;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyRepository _repository;

    public PropertiesController(IPropertyRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? name, [FromQuery] string? address,
                                           [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
    {
        var properties = await _repository.GetAllAsync(name, address, minPrice, maxPrice);
        return Ok(properties);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var property = await _repository.GetByIdAsync(id);
        if (property == null) return NotFound();
        return Ok(property);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Property property)
    {
        await _repository.AddAsync(property);
        return CreatedAtAction(nameof(GetById), new { id = property.IdOwner }, property);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, Property property)
    {
        if (id != property.IdOwner) return BadRequest();
        await _repository.UpdateAsync(property);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
