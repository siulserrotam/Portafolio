namespace API.Errors
{
    // Clase para manejar errores y respuestas de la API
    public class ApiResponse
    {
        // Código de estado HTTP que indica el tipo de error (ej. 400, 404, 500, etc.)
        public int StatusCode { get; set; } // Código de error

        // Mensaje que describe el error
        public string Message { get; set; } // Mensaje de error

        // Constructor que inicializa el código de estado y el mensaje del error
        // Si no se proporciona un mensaje, se usa el mensaje predeterminado según el código de estado
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;  // Asignar el código de estado
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);  // Usar el mensaje predeterminado si no se proporciona uno
        }

        // Método privado que devuelve un mensaje de error predeterminado según el código de estado
        // El switch maneja diferentes códigos HTTP y sus mensajes asociados
        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            // Dependiendo del código de estado, se devuelve un mensaje adecuado
            return statusCode switch
            {
                400 => "Has realizado una solicitud incorrecta.",  // 400: Solicitud incorrecta
                401 => "No estás autorizado para realizar esta acción.",  // 401: No autorizado
                404 => "El recurso que buscas no se ha encontrado.",  // 404: Recurso no encontrado
                500 => "Los errores son el camino hacia el lado oscuro. Los errores llevan a la ira." 
                +"La ira lleva al odio. El odio lleva al cambio de carrera.",  // 500: Error interno en el servidor
                _ => null  // Si el código no está definido, no se devuelve mensaje
            };
        }
    }
}
