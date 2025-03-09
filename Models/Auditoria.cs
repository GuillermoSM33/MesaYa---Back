using System.ComponentModel.DataAnnotations;

namespace MesaYa.Models
{
    public class Auditoria
    {
        [Key]
        public int AuditoriaId { get; set; }

        public int? UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        [Required, MaxLength(50)]
        public string Module { get; set; }

        [Required, MaxLength(100)]
        public string Accion { get; set; }

        public string? Detalle { get; set; }
        public DateTime FechaEvento { get; set; } = DateTime.Now;
    }
}
