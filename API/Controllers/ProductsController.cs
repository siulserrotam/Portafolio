using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic; // Para usar List<T>
using System.Linq; // Para usar ToList()

namespace API.Controller
{
    // Se define un controlador llamado ProductsController que hereda de ControllerBase
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext _context; // Declaración de _context

        // Constructor con inyección de dependencias
        public ProductsController(StoreContext context)
        {
            _context = context;
        }

        // Acción GET que devuelve una lista de productos
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();  // Obtener los productos de la base de datos
            return Ok(products);  // Devolver los productos con un código 200 OK
        }

        // Acción GET que devuelve un solo producto según el id
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _context.Products.FindAsync(id); // Buscar producto por id
        }
    }
}

