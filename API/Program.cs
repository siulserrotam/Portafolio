using Microsoft.EntityFrameworkCore;
using API.Data;  // Asegúrate de que StoreContext esté en este espacio de nombres
using Core.Interfaces;  // Asegúrate de que IProductRepository esté aquí
using Infrastructure.Data;  // Asegúrate de que ProductRepository esté aquí
using Microsoft.Extensions.DependencyInjection;  // Para la inyección de dependencias
using Microsoft.Extensions.Logging;  // Para el logger
using System.Threading.Tasks;  // Para tareas asincrónicas

var builder = WebApplication.CreateBuilder(args);

// Configuración del DbContext para usar SQL Server
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Usa SQL Server

// Registrar servicios para controladores
builder.Services.AddControllers();

// Registra el repositorio de productos
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Servicios de Swagger para la documentación
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar el logger factory a los servicios (necesario para la siembra de datos)
builder.Services.AddLogging();

var app = builder.Build();

// Configuración del pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usar HTTPS
app.UseHttpsRedirection();

// Mapea las rutas de los controladores
app.MapControllers();

// Aplicar migraciones al inicio
await ApplyMigrationsAsync(app);

app.Run();

// Método para aplicar migraciones al inicio
static async Task ApplyMigrationsAsync(WebApplication app)
{
    using (var scope = app.Services.CreateScope()) // Crear un scope para obtener servicios
    {
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>(); // Obtener el loggerFactory

        try
        {
            var context = services.GetRequiredService<StoreContext>(); // Obtener el contexto de datos
            await context.Database.MigrateAsync(); // Aplicar las migraciones de la base de datos
            await StoreContextSeed.SeedAsync(context, loggerFactory); // Sembrar los datos en la base de datos
        }
        catch (Exception ex)
        {
            // Si ocurre un error durante la migración, se registra el error en el log
            var logger = loggerFactory.CreateLogger<Program>(); // Crear logger para registrar el error
            logger.LogError(ex, "An error occurred during migration");
        }
    }
}
