using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MesaYa.Models
{
    public class Restaurante
    {
        [Key]
        public int RestauranteId { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } // Propiedad de navegación

        [Required, MaxLength(250)]
        public string RestauranteNombre { get; set; }

        [Required, MaxLength(250)]
        public string Direccion { get; set; }

        [ MaxLength(20)]
        public string Telefono { get; set; }

        public string ImagenUrl { get; set; }

        public TimeSpan HoraApertura { get; set; }
        public TimeSpan HoraCierre { get; set; }

        [Required]
        public string Horario { get; set; } // Ejemplo: "Mañana" o "Noche"

        public string Descripcion { get; set; }

        public bool IsDeleted { get; set; } = false; // Soft delete

        public ICollection<RestaurantesFavoritos> RestaurantesFavoritos { get; set; }

        public ICollection<ItemAsRestaurante> ItemAsRestaurantes { get; set; } = new List<ItemAsRestaurante>();


    }
}
