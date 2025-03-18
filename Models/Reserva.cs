using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MesaYa.Models
{
    public class Reserva
    {
        [Key]
        public int ReservaId { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [Required]
        public DateTime FechaReserva { get; set; }

        [Required, MaxLength(20)]
        public string Estado { get; set; }

        [Required]
        public int NumeroPersonas { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;

        public ICollection<ReservaAsMesa> ReservaAsMesas { get; set; } = new List<ReservaAsMesa>();
    }
}
