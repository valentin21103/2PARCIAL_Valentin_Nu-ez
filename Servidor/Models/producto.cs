using System.ComponentModel.DataAnnotations;

namespace Servidor.Models;

public class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria")]
    public string Descripcion { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int Stock { get; set; }

    [Required(ErrorMessage = "La URL de la imagen es obligatoria")]
    public string ImagenUrl { get; set; }
}