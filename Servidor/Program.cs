using Microsoft.EntityFrameworkCore;
using Servidor.Models;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios CORS para permitir solicitudes desde el cliente
builder.Services.AddCors(options => {
    options.AddPolicy("AllowClientApp", policy => {
        policy.WithOrigins("http://localhost:5177", "https://localhost:7221")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Agregar controladores si es necesario
builder.Services.AddControllers();

builder.Services.AddDbContext<TiendaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}

// Usar CORS con la política definida
app.UseCors("AllowClientApp");

// Mapear rutas básicas
app.MapGet("/", () => "Servidor API está en funcionamiento");

// Ejemplo de endpoint de API
app.MapGet("/api/datos", () => new { Mensaje = "Datos desde el servidor", Fecha = DateTime.Now });

// Endpoint para obtener productos
app.MapGet("/productos", async (TiendaContext db, string? query) => {
    var productos = db.Productos.AsQueryable();
    
    if (!string.IsNullOrWhiteSpace(query)) {
        query = query.ToLower();
        productos = productos.Where(p => 
            p.Nombre.ToLower().Contains(query) || 
            p.Descripcion.ToLower().Contains(query)
        );
    }
    
    return await productos.ToListAsync();
});

// Endpoint para obtener un producto específico
app.MapGet("/productos/{id}", async (TiendaContext db, int id) => {
    var producto = await db.Productos.FindAsync(id);
    if (producto == null) {
        return Results.NotFound();
    }
    return Results.Ok(producto);
});

// Endpoint para crear un nuevo carrito
app.MapPost("/carritos", async (TiendaContext db) => {
    var carrito = new Carrito
    {
        FechaCreacion = DateTime.Now
    };
    
    db.Carritos.Add(carrito);
    await db.SaveChangesAsync();
    
    return Results.Created($"/carritos/{carrito.Id}", carrito);
});

// Endpoint para obtener un carrito específico
app.MapGet("/carritos/{id}", async (TiendaContext db, int id) => {
    var carrito = await db.Carritos
        .Include(c => c.Items)  // Incluye los items del carrito
        .ThenInclude(i => i.Producto)  // Incluye los productos de cada item
        .FirstOrDefaultAsync(c => c.Id == id);

    if (carrito == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(carrito);
});

// Endpoint para eliminar un carrito
app.MapDelete("/carritos/{id}", async (TiendaContext db, int id) => {
    // 1. Buscar el carrito con sus items
    var carrito = await db.Carritos
        .Include(c => c.Items)
        .FirstOrDefaultAsync(c => c.Id == id);

    // 2. Verificar si existe
    if (carrito == null)
    {
        return Results.NotFound();
    }

    // 3. Eliminar todos los items
    db.ItemsCarrito.RemoveRange(carrito.Items);

    // 4. Guardar cambios
    await db.SaveChangesAsync();

    // 5. Devolver respuesta exitosa
    return Results.NoContent();
});

// Endpoint para actualizar un carrito
app.MapPut("/carritos/{id}/{productoId}", async (TiendaContext db, int id, int productoId, int cantidad) => {
    // 1. Buscar carrito y producto
    var carrito = await db.Carritos
        .Include(c => c.Items)
        .FirstOrDefaultAsync(c => c.Id == id);

    var producto = await db.Productos.FindAsync(productoId);

    // 2. Validar que existan
    if (carrito == null || producto == null)
    {
        return Results.NotFound();
    }

    // 3. Buscar si el producto ya está en el carrito
    var item = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);

    // 4. Actualizar o crear item
    if (item != null)
    {
        // Actualizar cantidad
        item.Cantidad = cantidad;
    }
    else
    {
        // Crear nuevo item
        item = new ItemCarrito
        {
            CarritoId = id,
            ProductoId = productoId,
            Cantidad = cantidad
        };
        carrito.Items.Add(item);
    }

    // 5. Validar stock
    if (item.Cantidad > producto.Stock)
    {
        return Results.BadRequest("No hay suficiente stock disponible");
    }

    // 6. Guardar cambios
    await db.SaveChangesAsync();

    return Results.Ok(carrito);
});

// Endpoint para eliminar un producto específico del carrito
app.MapDelete("/carritos/{id}/{productoId}", async (TiendaContext db, int id, int productoId) => {
    // 1. Buscar el carrito con sus items
    var carrito = await db.Carritos
        .Include(c => c.Items)
        .FirstOrDefaultAsync(c => c.Id == id);

    // 2. Verificar si existe el carrito
    if (carrito == null)
    {
        return Results.NotFound();
    }

    // 3. Buscar el item específico
    var item = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);
    if (item == null)
    {
        return Results.NotFound();
    }

    // 4. Eliminar el item
    carrito.Items.Remove(item);

    // 5. Guardar cambios
    await db.SaveChangesAsync();

    return Results.NoContent();
});

// Endpoint para confirmar compra
app.MapPut("/carritos/{id}/confirmar", async (TiendaContext db, int id, DatosCliente datos) => {
    // 1. Buscar el carrito con sus items y productos
    var carrito = await db.Carritos
        .Include(c => c.Items)
        .ThenInclude(i => i.Producto)
        .FirstOrDefaultAsync(c => c.Id == id);

    if (carrito == null || !carrito.Items.Any())
    {
        return Results.NotFound();
    }

    // 2. Crear la compra
    var compra = new Compra
    {
        Fecha = DateTime.Now,
        NombreCliente = datos.Nombre,
        ApellidoCliente = datos.Apellido,
        EmailCliente = datos.Email,
        Total = carrito.Items.Sum(i => i.Producto.Precio * i.Cantidad)
    };

    // 3. Crear los items de la compra y actualizar stock
    foreach (var item in carrito.Items)
    {
        // Verificar stock
        if (item.Cantidad > item.Producto.Stock)
        {
            return Results.BadRequest($"No hay suficiente stock de {item.Producto.Nombre}");
        }

        // Crear item de compra
        compra.ItemsCompra.Add(new ItemCompra
        {
            ProductoId = item.ProductoId,
            Cantidad = item.Cantidad,
            PrecioUnitario = item.Producto.Precio
        });

        // Actualizar stock
        item.Producto.Stock -= item.Cantidad;
    }

    // 4. Guardar la compra
    db.Compras.Add(compra);

    // 5. Vaciar el carrito
    db.ItemsCarrito.RemoveRange(carrito.Items);

    // 6. Guardar todos los cambios
    await db.SaveChangesAsync();

    return Results.Ok(compra);
});

app.Run();
