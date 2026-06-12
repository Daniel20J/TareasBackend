using System.ComponentModel.DataAnnotations;

namespace TareasApi.DTOs
{
    public class TareaCrearDto
    {
        [Required]
        public string Titulo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;
    }
}