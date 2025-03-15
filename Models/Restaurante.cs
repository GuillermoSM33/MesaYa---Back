using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MesaYa.Models
{
    public class Restaurante
    {
        [Key]
        public int RestauranteId { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } // Propiedad de navegación

        [Required]
        public string RestauranteNombre { get; set; }

        [Required]
        public string Direccion { get; set; }

        [Required]
        public string Telefono { get; set; }

        public string ImagenUrl { get; set; }

        [Required]
        public string Horario { get; set; } // Ejemplo: "Mañana" o "Noche"

        public string Descripcion { get; set; }


        public bool IsDeleted { get; set; } = false; // Soft delete
    }
}