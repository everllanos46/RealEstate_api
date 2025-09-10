
namespace RealEstate.Domain.Entities;

public class PropertyTrace : IEntity
{

    public string Id { get; set; } = string.Empty;
    public string IdPropertyTrace { get; set; } = string.Empty;
    public DateTime DateSale { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
    public string IdProperty { get; set; } = string.Empty;
}
