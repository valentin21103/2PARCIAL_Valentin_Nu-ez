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

app.Run();
