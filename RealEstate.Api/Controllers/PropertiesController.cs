using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
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
    public async Task<IActionResult> GetAllProperties(
    [FromQuery] PropertyQueryDto query)
    {
        try
        {
            var (properties, totalCount) = await _service.GetAllAsync(
       query.Nombre,
       query.Direccion,
       query.PrecioMinimo,
       query.PrecioMaximo,
       query.Pagina,
       query.TamanoPagina
   );

            return Ok(new { totalCount, properties });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Mensaje = "Ocurrió un error al obtener las propiedades.", Detalle = ex.Message });
        }
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePropertyDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var result = await _service.CreateAsync(request);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Ocurrió un error al crear la propiedad: {ex.Message}");
        }
    }

}
