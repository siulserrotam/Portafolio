using API.Errors;
using Core.Interfaces;
using Infraestructure.Data;
using Infrastructure.Data;  // Corrige el namespace aquí, "Infraestructure" parece ser un error tipográfico.
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Registra el repositorio de productos para inyección de dependencias
            services.AddScoped<IProductRepository, ProductRepository>(); // Registra la implementación del repositorio de productos

            // Registra el repositorio genérico para ser usado con cualquier tipo de entidad
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));  // Registra el repositorio genérico con inyección de dependencias
                        
            // Configuración personalizada para manejar errores de modelo en las respuestas de la API
            services.Configure<ApiBehaviorOptions>(Option => 
            {
                // Personalizamos la respuesta cuando el estado del modelo es inválido
                Option.InvalidModelStateResponseFactory = ActionContext => 
                {
                    // Extrae los mensajes de error del estado del modelo
                    var errors = ActionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    // Crea una respuesta de error con los mensajes de error
                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(errorResponse);  // Devuelve un error 400 BadRequest con los errores
                };
            });

            return services;
        }
    }
}
