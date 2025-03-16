
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MesaYa.Models
{
    public class Mesa
    {
        [Key]
        public int MesaId {get;set;}

        [Required]
        public int MesaNumero {get;set;}

        [ForeignKey("Restaurante")]
        public int RestauranteId { get; set; }      
        public Restaurante Restaurante { get; set; }
        public int Capacidad { get; set; }
        public bool Disponible { get; set; } = false;

        public bool IsDeleted { get; set; } = false;
    }
}
