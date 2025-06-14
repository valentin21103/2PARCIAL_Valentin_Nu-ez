using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servidor.Models;

public class Carrito
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; }

    // Relaciones
    public virtual ICollection<ItemCarrito> Items { get; set; } = new List<ItemCarrito>();
} 