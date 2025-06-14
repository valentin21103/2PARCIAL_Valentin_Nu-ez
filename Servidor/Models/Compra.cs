using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Servidor.Models;

public class Compra 
{
    [Key]
    public int Id { get; set; }
  
    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Total { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string NombreCliente { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio")]
    public string ApellidoCliente { get; set; }

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El email no es válido")]
    public string EmailCliente { get; set; }

    // Relación con ItemsCompra
    public virtual ICollection<ItemCompra> ItemsCompra { get; set; } = new List<ItemCompra>();
}