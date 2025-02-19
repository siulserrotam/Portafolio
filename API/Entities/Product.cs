namespace API.Entities  // Define el espacio de nombres 'API.Entities', que es donde se agrupan las entidades del modelo
{
    // Definición de la clase Product, que representa un producto en el sistema
    public class Product
    {
        // Propiedad que representa el identificador único del producto
        // 'Id' será la clave primaria en la base de datos para la tabla 'Products'
        public int Id { get; set; }

        // Propiedad que representa el nombre del producto
        // 'Name' será una columna de tipo cadena de texto en la base de datos para almacenar el nombre del producto
        public string Name { get; set; }
    }
}
