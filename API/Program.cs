using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración del DbContext para usar SQL Server
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Usa SQL Server

// Registrar servicios para controladores (esto habilita el uso de controladores en la aplicación)
builder.Services.AddControllers();

// Servicios de Swagger para la documentación
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usar HTTPS
app.UseHttpsRedirection();

// Mapea las rutas de los controladores (esto es necesario para que se reconozcan las rutas de API)
app.MapControllers();

app.Run();

