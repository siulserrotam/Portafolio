using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // Ruta para manejar errores con un código de estado HTTP personalizado
    [Route("errors/{code}")]
    public class ErrorController : BaseApiController
    {
        // Acción que devuelve una respuesta de error con un código de estado HTTP proporcionado
        public IActionResult Error(int code)
        {
            // Retorna un objeto con el código de error proporcionado y un mensaje de respuesta
            return new ObjectResult(new ApiResponse(code));
        }
    }
}
