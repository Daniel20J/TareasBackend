using System.ComponentModel.DataAnnotations;

namespace TareasApi.DTOs
{
    public class RegistroDto
    {
        [Required]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        public string UsuarioLogin { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}