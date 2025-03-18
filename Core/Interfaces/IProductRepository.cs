// Definición de la interfaz IProductRepository para interactuar con los productos, marcas y tipos de productos

// Usamos las entidades definidas en el espacio de nombres API.Entities y Core.Entities
using API.Entities;
using Core.Entities;
using Core.Specifications;

// Definimos la interfaz IProductRepository dentro del espacio de nombres Core.Interfaces
namespace Core.Interfaces
{
    // Esta interfaz define los métodos para interactuar con los productos, marcas y tipos de productos
    public interface IProductRepository
    {
        // Método asincrónico para obtener un producto por su ID
        // Devuelve un objeto Product que contiene la información de un producto específico
        Task<Product> GetProductByIdAsync(int id);

        // Método asincrónico para obtener todos los productos
        // Devuelve una lista de productos de solo lectura, sin permitir modificaciones directas
        Task<IReadOnlyList<Product>> GetProductsAsync();

        // Método asincrónico para obtener un producto con una especificación
        Task <Product> GetEntityWithSpecAsync(ISpecification<Product> spec);

        // Método asincrónico para obtener todas las marcas de productos
        // Devuelve una lista de marcas de productos de solo lectura
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();

        // Método asincrónico para obtener todos los tipos de productos
        // Devuelve una lista de tipos de productos de solo lectura
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();

        // Método asincrónico que toma una especificación y devuelve una lista de productos
        Task<IReadOnlyList<Product>> ListAsync(ProductsWithTypesAndBrandsSpecification spec);
    }
}
