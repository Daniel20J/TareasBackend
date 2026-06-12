namespace TareasApi.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expira { get; set; }
        public string UsuarioLogin { get; set; } = string.Empty;
    }
}