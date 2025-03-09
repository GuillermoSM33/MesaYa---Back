using System.ComponentModel.DataAnnotations;

namespace MesaYa.Models
{
    public class ConfiguracionSistema
    {
        [Key]
        public int ConfigId { get; set; }

        [Required, MaxLength(50)]
        public string ModuleName { get; set; }

        public bool IsActive { get; set; } = true;
        public string? Settings { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
