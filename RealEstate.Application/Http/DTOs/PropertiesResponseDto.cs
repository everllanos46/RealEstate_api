using RealEstate.Application.DTOs;

public class PropertiesResponseDto
{
    public IEnumerable<PropertyDto> Properties { get; set; } = Enumerable.Empty<PropertyDto>();
    public long TotalCount { get; set; }
}
