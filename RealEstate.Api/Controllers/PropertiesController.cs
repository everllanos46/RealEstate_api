using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.Services;

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
    public async Task<IActionResult> GetAllProperties([FromQuery] PropertyQueryDto query)
    {
        var response = await _service.GetAllAsync(
            query.Nombre,
            query.Direccion,
            query.PrecioMinimo,
            query.PrecioMaximo,
            query.Pagina,
            query.TamanoPagina
        );

        return StatusCode((int)response.HttpStatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePropertyDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _service.CreateAsync(request);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    [HttpGet("{propertyId}/get")]
    public async Task<IActionResult> GetById(string propertyId)
    {

        var response = await _service.GetByIdAsync(propertyId);
        return StatusCode((int)response.HttpStatusCode, response);
    }

    
}
