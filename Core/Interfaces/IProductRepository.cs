using API.Entities;
using Core.Entities;
namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);
        Task<IReadOnlyList<Product>> GetProductsAsync();
         Task<IReadOnlyList<ProductBrand>> GetProducBrandstsAsync();
        Task<IReadOnlyList<ProductType>> GetProductTypessAsync();
    }
}
