using API.Entities;
using Core.Entities;
using Core.Specifications;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification() 
            : base(x => true) // Aquí puedes poner tus filtros (por ejemplo, un producto activo)
        {
            AddInclude(x => x.ProductBrand); // Incluir marcas de productos
            AddInclude(x => x.ProductType); // Incluir tipos de productos
        }
    }
}

