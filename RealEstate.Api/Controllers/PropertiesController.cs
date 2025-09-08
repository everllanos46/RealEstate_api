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
    [FromQuery] string? nombre = null,
    [FromQuery] string? direccion = null,
    [FromQuery] decimal? precioMinimo = null,
    [FromQuery] decimal? precioMaximo = null,
    [FromQuery] int pagina = 1,
    [FromQuery] int tamanoPagina = 10)
    {
        try
        {
            var (propiedades, totalRegistros) = await _service.GetAllAsync(
                nombre, direccion, precioMinimo, precioMaximo, pagina, tamanoPagina);

            var respuesta = new
            {
                TotalRegistros = totalRegistros,
                PaginaActual = pagina,
                TamanoPagina = tamanoPagina,
                Propiedades = propiedades
            };

            return Ok(respuesta);
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
