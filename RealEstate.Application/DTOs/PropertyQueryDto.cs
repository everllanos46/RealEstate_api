namespace RealEstate.Application.DTOs;

public class PropertyQueryDto
{
    public string? Nombre { get; set; }
    public string? Direccion { get; set; }
    public decimal? PrecioMinimo { get; set; }
    public decimal? PrecioMaximo { get; set; }
    public int Pagina { get; set; } = 1;
    public int TamanoPagina { get; set; } = 10;
}
