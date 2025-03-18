using API.Controllers;
using API.Dtos;
using API.Entities;  // Importa el espacio de nombres donde están las entidades, como el modelo Product
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;  // Importa clases necesarias para crear controladores de la API en ASP.NET

namespace API.Controller
{

    public class ProductsController : BaseApiController
    {
        private readonly IProductRepository _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        // Constructor con la inyección de dependencias
        public ProductsController(
            IProductRepository productRepo, 
            IGenericRepository<ProductBrand> productBrandRepo, 
            IGenericRepository<ProductType> productTypeRepo,
            IMapper mapper)
        {
            _productRepo = productRepo;  // Asignamos las dependencias a las variables de instancia
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }

        // Método para obtener todos los productos
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();  // Definimos la especificación para obtener los productos

            var products = await _productRepo.ListAsync(spec);  // Usamos el repositorio para obtener los productos con la especificación

            // Si no hay productos, se retorna un 404 NotFound
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }

            // Se mapea la lista de productos a una lista de DTOs y se devuelve
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        // Método para obtener un producto por su id
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiResponse), 404)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);  // Creamos una especificación que use el id del producto

            var product = await _productRepo.GetEntityWithSpecAsync(spec);  // Obtenemos el producto con esa especificación

            // Si no se encuentra el producto, retornamos un 404 NotFound
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            // Si no hay productos, se retorna un 404 NotFound
            if (product == null) return NotFound(new ApiResponse(404));
           
            
        
            // Se mapea el producto a un DTO y se devuelve
            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }

        // Método para obtener todas las marcas de productos
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var productBrands = await _productBrandRepo.ListAllAsync();  // Obtenemos todas las marcas de productos

            // Si no hay marcas, retornar un 404 NotFound
            if (productBrands == null || !productBrands.Any())
            {
                return NotFound("No product brands found.");
            }

            return Ok(productBrands);
        }

        // Método para obtener todos los tipos de productos
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            var productTypes = await _productTypeRepo.ListAllAsync();  // Obtenemos todos los tipos de productos

            // Si no hay tipos, retornar un 404 NotFound
            if (productTypes == null || !productTypes.Any())
            {
                return NotFound("No product types found.");
            }

            return Ok(productTypes);
        }
    }
}
