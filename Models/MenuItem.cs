using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MesaYa.Models
{
    public class MenuItem
    {
        [Key]
        public int ItemId { get; set; }

        [ForeignKey("MenuCategoria")]
        public int CategoriaId { get; set; }
        public MenuCategoria MenuCategoria { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; }

        [MaxLength(255)]
        public string? Descripcion { get; set; }

        [Required]
        public decimal Precio { get; set; }

        public string? ImagenUrl { get; set; }
        public bool Disponible { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}
