using API.Data;
using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //Manejo de errores
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;  // Se debe declarar la variable _context

        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var thing = _context.Products.Find(42); // Buscamos un producto que no existe

            if (thing == null)
            {
                return NotFound(new ApiResponse(404)); // Devolver un error 404 si el objeto no fue encontrado
            }
            return Ok(thing); // Devolver el objeto encontrado si no es nulo
        }

        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
                var thing = _context.Products.Find(42); // Intentamos encontrar un producto que puede no existir

                if (thing == null)
                {
                    // Si 'thing' es nulo, devolvemos un error 500 con un mensaje personalizado
                    return StatusCode(500, "Producto no encontrado o algo salió mal.");
                }

                var productString = thing.ToString(); // Esto solo se ejecuta si 'thing' no es nulo

                return Ok(); // Devolvemos una respuesta exitosa

        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400)); // Devolver un error 400
        }

        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequestWithId(int id) // Cambié el nombre a algo más claro
        {
            return Ok(); // Devolver un error 400 si el id es menor o igual a 0
        }
    }
}
