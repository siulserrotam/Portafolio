// Configuración y puesta en marcha de la aplicación web, con la configuración de DbContext, repositorios e inyección de dependencias

// Usamos los espacios de nombres necesarios para configurar la aplicación
using Microsoft.EntityFrameworkCore;
using API.Data;  // Asegúrate de que StoreContext esté en este espacio de nombres
using Core.Interfaces;  // Asegúrate de que IProductRepository esté aquí
using Infrastructure.Data;  // Asegúrate de que ProductRepository esté aquí
using Infraestructure.Data;
using My_skiNet.API.Helpers;
using API.Middleware;  // Para tareas asincrónicas

// Crea y configura el builder para la aplicación web
var builder = WebApplication.CreateBuilder(args);

// Configuración del DbContext para usar SQL Server
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Usa SQL Server

// Registrar servicios para controladores
builder.Services.AddControllers();

// Registra el repositorio de productos
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Registra el repositorio genérico
// Para registrar el repositorio genérico, se debe registrar el tipo de repositorio genérico y el tipo de entidad
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);  // Configura AutoMapper

// Servicios de Swagger para la documentación
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar el logger factory a los servicios (necesario para la siembra de datos)
builder.Services.AddLogging();

// Construir la aplicación
var app = builder.Build();

// Middleware para manejar excepciones
app.UseMiddleware<ExceptionMiddleware>();

app.UseExceptionHandler("/errors/{0}");  // Manejar excepciones

// Usar HTTPS
app.UseHttpsRedirection();

// Usar el enrutamiento
app.UseRouting();

app.UseStaticFiles();  // Usar archivos estáticos

// Mapea las rutas de los controladores
app.MapControllers();

// Aplicar migraciones al inicio de la aplicación
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
