// Configuración y puesta en marcha de la aplicación web, con la configuración de DbContext, repositorios e inyección de dependencias

// Usamos los espacios de nombres necesarios para configurar la aplicación
using Microsoft.EntityFrameworkCore;  // Espacio de nombres necesario para trabajar con Entity Framework y SQL Server
using API.Data;  // Asegúrate de que StoreContext esté en este espacio de nombres (Contexto de datos de la tienda)
using Core.Interfaces;  // Asegúrate de que IProductRepository esté aquí (Interfaz del repositorio de productos)
using Infrastructure.Data;  // Asegúrate de que ProductRepository esté aquí (Implementación del repositorio de productos)
using Infraestructure.Data;  // Espacio de nombres adicional para la infraestructura de datos
using My_skiNet.API.Helpers;  // Espacio de nombres con clases de ayuda
using API.Middleware;  // Espacio de nombres para el middleware
using Microsoft.AspNetCore.Mvc;  // Espacio de nombres para trabajar con controladores y API
using API.Errors;
using Microsoft.OpenApi.Models;
using API.Extensions;
using MySkinet.API.Extensions;  // Para tareas asincrónicas y gestión de errores en la API

// Crea y configura el builder para la aplicación web
var builder = WebApplication.CreateBuilder(args); // Crea un nuevo constructor de la aplicación web

// Configuración del DbContext para usar SQL Server
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Configura el DbContext para conectarse a SQL Server con la cadena de conexión

builder.Services.AddApplicationServices();  // Registra los servicios de la aplicación para inyección de dependencias

builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "My_skiNet.API", Version = "v1" });
    }    
);  // Registra Swagger para generar la documentación de la API

// Registrar servicios para controladores
builder.Services.AddControllers();  // Registra los controladores para la API

// Configura AutoMapper para la mapeo de objetos entre diferentes tipos de clases
builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);  // Configura AutoMapper a partir de los perfiles de mapeo

// Servicios de Swagger para generar documentación interactiva de la API
builder.Services.AddEndpointsApiExplorer();  // Habilita la exploración de puntos finales de la API
builder.Services.AddSwaggerGen();  // Registra Swagger para generar la documentación de la API

// Agregar el logger factory a los servicios (necesario para la siembra de datos)
builder.Services.AddLogging();  // Registra los servicios de logging para capturar eventos y errores

// Construir la aplicación
var app = builder.Build();  // Construye la aplicación a partir de la configuración del builder

app.UseSwaggerDocumentation();  // Configura Swagger para la documentación de la API

// Middleware para manejar excepciones
app.UseMiddleware<ExceptionMiddleware>();  // Configura el middleware para manejar excepciones personalizadas

// Configura el manejador de excepciones en la aplicación
app.UseExceptionHandler("/errors/{0}");  // Redirige a la página de errores si se produce una excepción

// Usar HTTPS
app.UseHttpsRedirection();  // Redirige todo el tráfico HTTP a HTTPS para mayor seguridad

// Usar el enrutamiento para manejar las solicitudes
app.UseRouting();  // Habilita el enrutamiento de las solicitudes HTTP hacia los controladores

app.UseStaticFiles();  // Configura la aplicación para servir archivos estáticos (por ejemplo, imágenes, JavaScript, CSS)

// Mapea las rutas de los controladores a las solicitudes HTTP
app.MapControllers();  // Registra las rutas de los controladores en la aplicación

// Aplicar migraciones al inicio de la aplicación (para asegurar que la base de datos esté actualizada)
await ApplyMigrationsAsync(app);  // Aplica las migraciones pendientes y siembra la base de datos si es necesario

// Inicia la aplicación web
app.Run();  // Corre la aplicación web

// Método para aplicar migraciones al inicio de la aplicación
static async Task ApplyMigrationsAsync(WebApplication app)
{
    using (var scope = app.Services.CreateScope())  // Crea un scope para obtener servicios desde el contenedor de dependencias
    {
        var services = scope.ServiceProvider;  // Obtiene el proveedor de servicios del scope
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();  // Obtiene la factoría de logs para registrar eventos

        try
        {
            var context = services.GetRequiredService<StoreContext>();  // Obtiene el contexto de la base de datos StoreContext
            await context.Database.MigrateAsync();  // Aplica las migraciones pendientes a la base de datos
            await StoreContextSeed.SeedAsync(context, loggerFactory);  // Siembra datos iniciales en la base de datos
        }
        catch (Exception ex)
        {
            // Si ocurre un error durante el proceso de migración, lo registramos en el log
            var logger = loggerFactory.CreateLogger<Program>();  // Crea un logger para registrar el error
            logger.LogError(ex, "An error occurred during migration");  // Registra el error de migración
        }
    }
}
