using API.Entities;  // Importa el espacio de nombres donde están las entidades, como el modelo Product
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;  // Importa clases necesarias para crear controladores de la API en ASP.NET

namespace API.Controller
{
    // Se define un controlador llamado ProductsController que hereda de ControllerBase
    // [ApiController] habilita ciertas configuraciones automáticas de la API, como la validación de modelos
    [ApiController]
    // Define la ruta base del controlador, donde el nombre del controlador se usará como parte de la URL
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly IProductRepository _ProductRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;

       public ProductsController(
                IProductRepository productRepo, 
                IGenericRepository<ProductBrand> productBrandRepo, 
                IGenericRepository<ProductType> productTypeRepo)
        {
            _ProductRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();

            var products = await _ProductRepo.ListAsync(spec);
            
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
           
            return await _ProductRepo.GetProductByIdAsync(id); 
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandRepo.ListAllAsync());
        }

            [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }
    }

}