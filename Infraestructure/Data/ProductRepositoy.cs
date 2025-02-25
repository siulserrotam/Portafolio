using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using API.Entities;
using API.Data;
using Core.Entities;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;

        // Constructor que recibe el contexto de base de datos
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProducBrandstsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        // Método para obtener un producto por su ID
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)   
                .FirstOrDefaultAsync(p => p.Id == id); 
             
        }

        // Método para obtener todos los productos
        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypessAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}
