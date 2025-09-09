
namespace RealEstate.Domain.Entities;

public class Owner : IEntity
{
    public string Id { get; set; } = string.Empty;
    public string IdOwner { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
}