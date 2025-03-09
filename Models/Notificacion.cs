using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MesaYa.Models
{
    public class Notificacion
    {
        [Key]
        public int NotificacionId { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        [Required, MaxLength(255)]
        public string Mensaje { get; set; }

        [Required, MaxLength(50)]
        public string Tipo { get; set; }

        public bool Enviado { get; set; } = false;
        public DateTime? FechaEnvio { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
