namespace RealEstate.Application.DTOs;

public class CreatePropertyDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string IdOwner { get; set; } = string.Empty;

}
