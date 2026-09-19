using System.ComponentModel.DataAnnotations;

namespace ApiProductos.DTOs;

public class GuardarProductoDto
{
    private string producto = string.Empty;
    [Required(ErrorMessage = "El producto es obligatorio.")]
    [StringLength(50)]
    public string Producto { get => producto; set => producto = value?.Trim() ?? string.Empty; }

    [Required]
    [Range(1, short.MaxValue, ErrorMessage = "Seleccione una marca.")]
    public short? IdMarca { get; set; }

    private string? descripcion;
    [StringLength(100)]
    public string? Descripcion { get => descripcion; set => descripcion = string.IsNullOrWhiteSpace(value) ? null : value.Trim(); }

    [Range(typeof(decimal), "0", "999999.99")]
    [Required]
    public decimal? PrecioCosto { get; set; }

    [Range(typeof(decimal), "0", "999999.99")]
    [Required]
    public decimal? PrecioVenta { get; set; }

    [Range(0, int.MaxValue)]
    [Required]
    public int? Existencia { get; set; }
}
