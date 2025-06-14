using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servidor.Models;

public class ItemCompra
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CompraId { get; set; }

    [Required]
    public int ProductoId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal PrecioUnitario { get; set; }

    // Relaciones
    [ForeignKey("CompraId")]
    public virtual Compra Compra { get; set; } = null!;

    [ForeignKey("ProductoId")]
    public virtual Producto Producto { get; set; } = null!;
}