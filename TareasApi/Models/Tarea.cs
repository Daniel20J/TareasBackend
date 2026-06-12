using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TareasApi.Models
{
    public class Tarea
    {
       public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        public bool Completada { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [ForeignKey(nameof(Usuario))]
        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; } 
    }
}