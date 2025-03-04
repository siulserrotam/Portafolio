namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        // Constructor que recibe los parámetros y pasa el código de estado y mensaje a la clase base
        public ApiException(int statusCode, string message = null, string details = null)
            : base(statusCode, message)
        {
            Details = details; // Asigna los detalles adicionales del error
        }

        // Propiedad para los detalles adicionales del error
        public string Details { get; set; }
    }
}
