using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        // Constructor para inicializar las dependencias
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        // Método para manejar excepciones durante el procesamiento de la solicitud
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Llamar al siguiente middleware en la cadena
                await _next(context);
            }
            catch (Exception ex)
            {
                // Registrar el error
                _logger.LogError(ex, ex.Message);

                // Establecer el tipo de contenido y el código de estado de la respuesta
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Crear la respuesta dependiendo del entorno (Desarrollo o Producción)
                var response = _env.IsDevelopment()
                    ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiException((int)HttpStatusCode.InternalServerError);

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                // Enviar la respuesta al cliente
                await context.Response.WriteAsync(json);
            }
        }
    }
}