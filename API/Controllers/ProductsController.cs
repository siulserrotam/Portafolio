using API.Data;  // Importa el espacio de nombres donde está el contexto de la base de datos (StoreContext)
using API.Entities;  // Importa el espacio de nombres donde están las entidades, como el modelo Product
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;  // Importa clases necesarias para crear controladores de la API en ASP.NET
using Microsoft.EntityFrameworkCore;  // Importa clases necesarias para trabajar con Entity Framework (acceso a datos)
using System.Collections.Generic; // Para usar List<T> que se utiliza para devolver colecciones de objetos
using System.Linq; // Para usar métodos de LINQ, como ToList()


namespace API.Controller
{
    // Se define un controlador llamado ProductsController que hereda de ControllerBase
    // [ApiController] habilita ciertas configuraciones automáticas de la API, como la validación de modelos
    [ApiController]
    // Define la ruta base del controlador, donde el nombre del controlador se usará como parte de la URL
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        // Declaración del campo privado para el repositorio
        private readonly IProductRepository _repo;

        // Constructor que recibe el repositorio como parámetro
        public ProductsController(IProductRepository repo)
        {
            _repo = repo;   
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            // Se obtiene la lista de productos desde la base de datos de manera asincrónica
            var products = await _repo.GetProductsAsync(); 

            // Se devuelve la lista de productos con una respuesta HTTP 200 OK
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
           
            return await _repo.GetProductByIdAsync(id); 
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _repo.GetProducBrandstsAsync());
        }

            [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _repo.GetProducBrandstsAsync());
        }
    }
}
