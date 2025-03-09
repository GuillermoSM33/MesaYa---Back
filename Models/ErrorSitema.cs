using System.ComponentModel.DataAnnotations;

namespace MesaYa.Models
{
    public class ErrorSistema
    {
        [Key]
        public int ErrorId { get; set; }

        [Required, MaxLength(50)]
        public string Module { get; set; }

        [Required, MaxLength(255)]
        public string ErrorMessage { get; set; }

        public string? StackTrace { get; set; }
        public DateTime FechaError { get; set; } = DateTime.Now;
        public bool Resuelto { get; set; } = false;
    }
}
