using API.Entities;  // Este namespace hace referencia a las entidades del modelo, en este caso 'Product'
using Microsoft.EntityFrameworkCore;  // Este namespace contiene las clases y métodos para trabajar con Entity Framework Core

namespace API.Data  // Definimos el espacio de nombres 'API.Data' donde se manejará el contexto de la base de datos
{
    // Definición de la clase StoreContext que hereda de DbContext
    public class StoreContext : DbContext
    {
        // Constructor que recibe un objeto DbContextOptions que contiene la configuración de la base de datos
        // El constructor pasa las opciones al constructor base de DbContext
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        // Propiedad DbSet<Product>? que representa la tabla 'Products' en la base de datos
        // 'Product' es una clase que representa una entidad de la base de datos
        // DbSet se utiliza para trabajar con una colección de elementos de esa entidad.
        public DbSet<Product>? Products { get; set; }
    }
}
