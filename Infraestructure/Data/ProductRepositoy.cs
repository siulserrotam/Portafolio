// Implementación del repositorio de productos para interactuar con la base de datos utilizando Entity Framework Core

using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using API.Entities;
using API.Data;
using Core.Entities;
using Core.Specifications;

namespace Infrastructure.Data
{
    // Implementación de la interfaz IProductRepository
    public class ProductRepository : IProductRepository
    {
        // Contexto de base de datos utilizado para interactuar con los datos
        private readonly StoreContext _context;

        // Constructor que recibe el contexto de base de datos (StoreContext)
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        // Método para obtener todas las marcas de productos
        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            // Devuelve todas las marcas de productos de la base de datos de manera asincrónica
            return await _context.ProductBrands.ToListAsync();
        }

        // Método para obtener un producto específico por su ID
        public async Task<Product> GetProductByIdAsync(int id)
        {
            // Busca un producto que tenga el mismo ID, incluyendo su marca y tipo
            return await _context.Products
                .Include(p => p.ProductBrand)  // Incluye la información de la marca del producto
                .Include(p => p.ProductType)   // Incluye la información del tipo del producto
                .FirstOrDefaultAsync(p => p.Id == id); // Busca el primer producto que coincida con el ID
        }

        // Método para obtener todos los productos
        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            // Devuelve todos los productos, incluyendo la información de marca y tipo
            return await _context.Products
                .Include(p => p.ProductBrand)  // Incluye la marca del producto
                .Include(p => p.ProductType)   // Incluye el tipo del producto
                .ToListAsync();  // Devuelve la lista completa de productos
        }

        // Método para obtener todos los tipos de productos
        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            // Devuelve todos los tipos de productos de la base de datos de manera asincrónica
            return await _context.ProductTypes.ToListAsync();
        }

        // Implementación del método ListAsync que utiliza la especificación
        public async Task<IReadOnlyList<Product>> ListAsync(ProductsWithTypesAndBrandsSpecification spec)
        {
            // Aplicando la especificación de la consulta con las inclusiones de marcas y tipos
            return await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Where(spec.Criteria)  // Aplica los filtros definidos en la especificación
                .ToListAsync();  // Ejecuta la consulta y devuelve la lista de productos
        }
    }
}
