using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.Data;  // Namespace para el contexto de la base de datos.
using API.Entities;  // Entidades del modelo de datos.
using Core.Entities;  // Definición de las entidades como Product, ProductBrand, etc.
using Microsoft.Extensions.Logging;  // Para el registro de errores y eventos.

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        // Método estático asincrónico para sembrar los datos en la base de datos.
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                // Verifica si ya existen productos en la base de datos.
                if (!context.Products.Any())
                {
                    // Si no existen productos, lee el archivo JSON que contiene los datos de los productos.
                    var productsData = 
                        File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                        
                    // Deserializa el contenido del archivo JSON a una lista de objetos Product.
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    // Itera a través de los productos deserializados y los agrega a la base de datos.
                    foreach (var item in products)
                    {
                        context.Products.Add(item);
                    }

                    // Guarda los cambios en la base de datos de forma asincrónica.
                    await context.SaveChangesAsync();
                }

                // Verifica si ya existen marcas de productos en la base de datos.
                if (!context.ProductBrands.Any())
                {
                    // Si no existen marcas, lee el archivo JSON que contiene los datos de las marcas.
                    var brandsData = 
                        File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");

                    // Deserializa el contenido del archivo JSON a una lista de objetos ProductBrand.
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    // Itera a través de las marcas deserializadas y las agrega a la base de datos.
                    foreach (var item in brands)
                    {
                        context.ProductBrands.Add(item);
                    }

                    // Guarda los cambios en la base de datos de forma asincrónica.
                    await context.SaveChangesAsync();
                }

                // Verifica si ya existen tipos de productos en la base de datos.
                if (!context.ProductTypes.Any())
                {
                    // Si no existen tipos, lee el archivo JSON que contiene los datos de los tipos de productos.
                    var typesData = 
                        File.ReadAllText("../Infrastructure/Data/SeedData/types.json");

                    // Deserializa el contenido del archivo JSON a una lista de objetos ProductType.
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    // Itera a través de los tipos de productos deserializados y los agrega a la base de datos.
                    foreach (var item in types)
                    {
                        context.ProductTypes.Add(item);
                    }

                    // Guarda los cambios en la base de datos de forma asincrónica.
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Si ocurre algún error, se captura la excepción y se registra en el log.
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);  // Registra el mensaje de error.
            }
        }

        public static async Task SeedAsync(StoreContext context, object loggerFactory)
        {
            throw new NotImplementedException();
        }
    }
}
