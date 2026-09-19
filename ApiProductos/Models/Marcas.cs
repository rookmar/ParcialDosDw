using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiProductos.Models;

    [Table("Marcas", Schema = "dbo" )]
    public class Marcas
    {
        [Key]
    [Column("idMarca")]
    public short IdMarca { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("marca")]
    public string Nombre { get; set; } = string.Empty;

    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }