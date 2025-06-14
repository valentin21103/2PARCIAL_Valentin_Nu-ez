using Microsoft.EntityFrameworkCore;
using Servidor.Models; 

public class TiendaContext : DbContext
{
    public TiendaContext(DbContextOptions<TiendaContext> options) : base(options)
    {
    }

    public DbSet<Producto> Productos { get; set; }
    public DbSet<Compra> Compras { get; set; }
    public DbSet<ItemCompra> ItemsCompra { get; set; }

    public void CargarDatosIniciales()
    {
        // Si no hay productos, cargar datos de ejemplo
        if (!Productos.Any())
        {
            Productos.AddRange(
                new Producto { Nombre = "iPhone 13", Descripcion = "Último modelo de Apple", Precio = 999.99m, Stock = 10, Imagen = "iphone13.jpg" },
                new Producto { Nombre = "Samsung Galaxy S21", Descripcion = "Potente smartphone Android", Precio = 899.99m, Stock = 15, Imagen = "galaxys21.jpg" },
                new Producto { Nombre = "MacBook Pro", Descripcion = "Laptop profesional", Precio = 1299.99m, Stock = 8, Imagen = "macbookpro.jpg" },
                new Producto { Nombre = "AirPods Pro", Descripcion = "Audífonos inalámbricos con cancelación de ruido", Precio = 249.99m, Stock = 20, Imagen = "airpodspro.jpg" },
                new Producto { Nombre = "iPad Air", Descripcion = "Tablet versátil", Precio = 599.99m, Stock = 12, Imagen = "ipadair.jpg" },
                new Producto { Nombre = "Sony WH-1000XM4", Descripcion = "Audífonos premium con cancelación de ruido", Precio = 349.99m, Stock = 10, Imagen = "sonywh1000xm4.jpg" },
                new Producto { Nombre = "Nintendo Switch", Descripcion = "Consola portátil", Precio = 299.99m, Stock = 15, Imagen = "nintendoswitch.jpg" },
                new Producto { Nombre = "Dell XPS 13", Descripcion = "Laptop ultrabook", Precio = 1199.99m, Stock = 7, Imagen = "dellxps13.jpg" },
                new Producto { Nombre = "Google Pixel 6", Descripcion = "Smartphone con cámara excepcional", Precio = 699.99m, Stock = 10, Imagen = "googlepixel6.jpg" },
                new Producto { Nombre = "Fitbit Charge 5", Descripcion = "Reloj inteligente para fitness", Precio = 179.99m, Stock = 20, Imagen = "fitbitcharge5.jpg" }
            );
            SaveChanges();
        }
    }
}

public