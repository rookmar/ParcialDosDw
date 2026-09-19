using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProductos.Models;

[Table("Productos", Schema = "dbo")]
public class Producto
{
    [Key]
    [Column("idProducto")]
    public int IdProducto { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("producto")]
    public string Nombre { get; set; } = string.Empty;

    [Column("idMarca")]
    public short IdMarca { get; set; }

    [MaxLength(100)]
    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("precioCosto", TypeName = "decimal(8,2)")]
    public decimal PrecioCosto { get; set; }

    [Column("precioVenta", TypeName = "decimal(8,2)")]
    public decimal PrecioVenta { get; set; }

    [Column("existencia")]
    public int Existencia { get; set; }

    [ForeignKey(nameof(IdMarca))]
    public Marcas? Marca { get; set; }
}