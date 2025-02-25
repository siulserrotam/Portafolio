using System.ComponentModel.DataAnnotations;
using Core.Entities;

namespace API.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }

        // Relación con ProductType
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }

        // Relación con ProductBrand
        public ProductBrand ProductBrand { get; set; }
        public int ProductBrandId { get; set; }
    }
}
