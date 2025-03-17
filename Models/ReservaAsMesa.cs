using System.ComponentModel.DataAnnotations.Schema;

namespace MesaYa.Models
{
    public class ReservaAsMesa
    {
        [ForeignKey("Reserva")]
        public int ReservaId { get; set; }
        public Reserva Reserva { get; set; }  // Propiedad de navegación a Reserva

        [ForeignKey("Mesa")]
        public int MesaId { get; set; }
        public Mesa Mesa { get; set; }  // Propiedad de navegación a Mesa
    }
}