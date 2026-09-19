using System.ComponentModel.DataAnnotations;

namespace ApiProductos.DTOs;

public class ProductoDto
{
    public int IdProducto { get; set; }
    public string Producto { get; set; } = string.Empty;
    public short IdMarca { get; set; }
    public string Marca { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal PrecioCosto { get; set; }
    public decimal PrecioVenta { get; set; }
    public int Existencia { get; set; }
}
