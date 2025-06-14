namespace Servidor.Models;

public class Producto
{
    public int id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public string Imagen { get; set; }

    
}