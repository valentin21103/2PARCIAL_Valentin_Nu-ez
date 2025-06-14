using Microsoft.EntityFrameworkCore;

namespace Servidor.Models;

public class TiendaContext : DbContext
{
    public TiendaContext(DbContextOptions<TiendaContext> options) : base(options) 
    {
        Productos = Set<Producto>();
        Carritos = Set<Carrito>();
        ItemsCarrito = Set<ItemCarrito>();
        Compras = Set<Compra>();
        ItemsCompra = Set<ItemCompra>();
    }

    public DbSet<Producto> Productos { get; set; }
    public DbSet<Carrito> Carritos { get; set; }
    public DbSet<ItemCarrito> ItemsCarrito { get; set; }
    public DbSet<Compra> Compras { get; set; }
    public DbSet<ItemCompra> ItemsCompra { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar relaciones
        modelBuilder.Entity<Carrito>()
            .HasMany(c => c.Items)
            .WithOne(i => i.Carrito)
            .HasForeignKey(i => i.CarritoId);

        modelBuilder.Entity<ItemCarrito>()
            .HasOne(i => i.Producto)
            .WithMany()
            .HasForeignKey(i => i.ProductoId);

        modelBuilder.Entity<Compra>()
            .HasMany(c => c.ItemsCompra)
            .WithOne(i => i.Compra)
            .HasForeignKey(i => i.CompraId);

        modelBuilder.Entity<ItemCompra>()
            .HasOne(i => i.Producto)
            .WithMany()
            .HasForeignKey(i => i.ProductoId);
    }

    public void CargarDatosIniciales()
    {
        if (!Productos.Any())
        {
            Productos.AddRange(
                new Producto { Nombre = "iPhone 15 Pro", Descripcion = "Último modelo de Apple con chip A17 Pro", Precio = 999.99m, Stock = 10, ImagenUrl = "iphone15pro.jpg" },
                new Producto { Nombre = "Samsung Galaxy S24", Descripcion = "Smartphone Android con IA integrada", Precio = 899.99m, Stock = 15, ImagenUrl = "galaxys24.jpg" },
                new Producto { Nombre = "MacBook Pro M3", Descripcion = "Laptop profesional con chip M3", Precio = 1299.99m, Stock = 8, ImagenUrl = "macbookprom3.jpg" },
                new Producto { Nombre = "AirPods Pro 2", Descripcion = "Audífonos inalámbricos con cancelación de ruido activa", Precio = 249.99m, Stock = 20, ImagenUrl = "airpodspro2.jpg" },
                new Producto { Nombre = "iPad Pro M2", Descripcion = "Tablet profesional con chip M2", Precio = 899.99m, Stock = 12, ImagenUrl = "ipadprom2.jpg" },
                new Producto { Nombre = "Sony WH-1000XM5", Descripcion = "Audífonos premium con cancelación de ruido líder", Precio = 399.99m, Stock = 10, ImagenUrl = "sonywh1000xm5.jpg" },
                new Producto { Nombre = "Nintendo Switch OLED", Descripcion = "Consola portátil con pantalla OLED", Precio = 349.99m, Stock = 15, ImagenUrl = "nintendoswitcholed.jpg" },
                new Producto { Nombre = "Dell XPS 15", Descripcion = "Laptop premium con pantalla OLED", Precio = 1499.99m, Stock = 7, ImagenUrl = "dellxps15.jpg" },
                new Producto { Nombre = "Google Pixel 8 Pro", Descripcion = "Smartphone con cámara y IA avanzada", Precio = 899.99m, Stock = 10, ImagenUrl = "pixel8pro.jpg" },
                new Producto { Nombre = "Apple Watch Series 9", Descripcion = "Reloj inteligente con chip S9", Precio = 399.99m, Stock = 20, ImagenUrl = "applewatch9.jpg" }
            );
            SaveChanges();
        }
    }
} 