using System.ComponentModel.DataAnnotations;

namespace MesaYa.Models
{
    public class MenuCategoria
    {
        [Key]
        public int CategoriaId { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; }
    }

}
