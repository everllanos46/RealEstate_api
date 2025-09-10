using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs;
using RealEstate.Application.Services;

namespace RealEstate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class PropertyTraceController : ControllerBase
{
    private readonly PropertyTraceService _propertyService;
    public PropertyTraceController(PropertyTraceService service)
    {
        _propertyService = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePropertyTraceDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _propertyService.CreateAsync(request);
        return StatusCode((int)response.HttpStatusCode, response);
    }
}