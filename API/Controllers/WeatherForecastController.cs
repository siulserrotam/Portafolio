using Microsoft.AspNetCore.Mvc;  // Importa las clases necesarias para trabajar con ASP.NET Core MVC y crear controladores RESTful.
using System.Collections.Generic;  // Importa las clases necesarias para trabajar con colecciones como List<T> o IEnumerable<T>.

namespace API.Controllers;  // Define el espacio de nombres del controlador, que es la ubicación lógica dentro del proyecto.

[ApiController]  // Indica que esta clase es un controlador de una API RESTful. Automáticamente habilita algunas características como la validación del modelo.
[ApiExplorerSettings(IgnoreApi = true)]  // Esta configuración hace que el controlador sea ignorado en la documentación de la API generada, como Swagger.
[Route("[controller]")]  // Define la ruta base para este controlador, el nombre de la clase se usará en la URL. En este caso, la ruta será "weatherforecast".
public class WeatherForecastController : ControllerBase  // Define el controlador WeatherForecastController que hereda de ControllerBase.
{
    // Lista estática que contiene los diferentes tipos de clima que se pueden generar aleatoriamente.
    private static readonly string[] Summaries = new[] 
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    // Declara un objeto logger para registrar información, advertencias, errores, etc., durante la ejecución.
    private readonly ILogger<WeatherForecastController> _logger;

    // Constructor que recibe un logger y lo asigna a la propiedad _logger.
    // Este logger será utilizado para generar registros de la actividad del controlador.
    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    // Acción GET que maneja las solicitudes GET a la ruta "weatherforecast" (ya que [Route("[controller]") lo convierte en la ruta "/WeatherForecast").
    // Genera una lista de objetos WeatherForecast con un rango de 5 días de pronóstico.
    [HttpGet(Name = "GetWeatherForecast")]  // El atributo HttpGet indica que esta acción maneja solicitudes GET.
    
    public IEnumerable<WeatherForecast> Get()
    {
        // Devuelve una lista de pronósticos de clima generados aleatoriamente con fechas y resúmenes.
        return Enumerable.Range(1, 5)  // Crea un rango de números del 1 al 5 (5 días).
            .Select(index => new WeatherForecast  // Crea un nuevo objeto WeatherForecast para cada número del rango.
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),  // Fecha de cada pronóstico (día actual + días del rango).
                TemperatureC = Random.Shared.Next(-20, 55),  // Temperatura aleatoria entre -20 y 55 grados Celsius.
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]  // Resumen de clima aleatorio tomado de la lista "Summaries".
            })
            .ToArray();  // Convierte la secuencia generada en un arreglo.
    }
}

// Clase que representa el pronóstico del tiempo para un día específico.
public class WeatherForecast
{
    public DateOnly Date { get; set; }  // Propiedad para almacenar la fecha del pronóstico (solo la fecha sin la hora).
    public int TemperatureC { get; set; }  // Propiedad para almacenar la temperatura en grados Celsius.
    public string Summary { get; set; }  // Propiedad para almacenar un resumen del estado del clima (por ejemplo, "Chilly", "Hot").
}
