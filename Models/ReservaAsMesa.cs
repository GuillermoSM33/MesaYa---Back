using System.ComponentModel.DataAnnotations.Schema;

namespace MesaYa.Models
{
    public class ReservaAsMesa
    {
        [ForeignKey("Reserva")]
        public int ReservaId { get; set; }
        public Reserva Reserva { get; set; }

        [ForeignKey("Mesas")]

        public int MesaId { get; set; }
        public Mesa Mesas { get; set; }
    }
}
