using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servidor.Models;

public class ItemCarrito
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CarritoId { get; set; }

    [Required]
    public int ProductoId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Cantidad { get; set; }

    // Relaciones
    [ForeignKey("CarritoId")]
    public virtual Carrito Carrito { get; set; } = null!;

    [ForeignKey("ProductoId")]
    public virtual Producto Producto { get; set; } = null!;
} 