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
}