namespace Servidor.Models;

public class ItemsCompra {
    public int id { get; set; }

    public int idCompra { get; set; }

    public int idProducto { get; set; }

    public int cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }
}