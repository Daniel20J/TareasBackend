namespace TareasApi.DTOs
{
    public class ApiResponseDto<T>
    {
        public bool Estatus { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public T? Data {  get; set; }
    }
}