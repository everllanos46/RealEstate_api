namespace RealEstate.Application.DTOs;

public class PropertyDto
{
    public string IdProperty { get; set; } = string.Empty;
    public string IdOwner { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public OwnerDto? Owner { get; set; } 
    public PropertyTraceDto? PropertyTraceDto { get; set; } 
}
