using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{
    // Se define un controlador llamado ProductsController que hereda de ControllerBase
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        // Acción GET que devuelve una cadena con un mensaje indicando que se obtendrán productos
        [HttpGet]
        public string GetProducts()
        {
            return "This will be a list of products"; // Este es un ejemplo de cómo se devolvería una lista de productos
        }

        // Acción GET que devuelve una cadena con un mensaje indicando que se obtendrá un solo producto
        [HttpGet ("{id}")]
        public string GetProduct(int id)
        {
            return "Single product"; // Este es un ejemplo de cómo se devolvería un producto individual
        }
    }
}
