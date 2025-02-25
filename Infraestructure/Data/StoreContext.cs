using System.Reflection;
using API.Entities;  // Este namespace hace referencia a las entidades del modelo, en este caso 'Product'.
using Core.Entities;
using Microsoft.EntityFrameworkCore;  // Este namespace contiene las clases y métodos para trabajar con Entity Framework Core.

namespace API.Data  // Definimos el espacio de nombres 'API.Data' donde se manejará el contexto de la base de datos.
{
    // Definición de la clase StoreContext que hereda de DbContext.
    public class StoreContext : DbContext
    {
        // Constructor que recibe un objeto DbContextOptions que contiene la configuración de la base de datos.
        // El constructor pasa las opciones al constructor base de DbContext.
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        // Propiedad DbSet<Product> que representa la tabla 'Products' en la base de datos.
        // 'Product' es una clase que representa una entidad de la base de datos.
        // DbSet se utiliza para trabajar con una colección de elementos de esa entidad.
        public DbSet<Product> Products { get; set; }  // Se eliminó el '?' ya que no es necesario si la propiedad siempre se inicializa correctamente.

        // Definición de otras tablas en la base de datos relacionadas con productos.
        public DbSet<ProductBrand> ProductBrands { get; set; }  // Representa la tabla 'ProductBrands' en la base de datos.
        public DbSet<ProductType> ProductTypes { get; set; }    // Representa la tabla 'ProductTypes' en la base de datos.

        // Método OnModelCreating que se usa para configurar las entidades y sus relaciones en el modelo.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // Llama al método base para mantener la configuración por defecto.
            
            // Aplica las configuraciones de las entidades en el ensamblado actual.
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());  
        } 
    }
}
