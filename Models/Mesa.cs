
using System.ComponentModel.DataAnnotations;

namespace MesaYa.Models
{
    public class Mesa
    {
        [Key]
        public int MesaId {get;set;}
        public int MesaNumero {get;set;}
        public int RestauranteId { get; set; }
        public int CantidadPersonas { get; set; }
        public bool Disponible { get; set; } = false;
    }
}
