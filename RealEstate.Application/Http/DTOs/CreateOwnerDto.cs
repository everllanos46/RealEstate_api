namespace RealEstate.Application.DTOs;

public class CreateOwnerDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
}