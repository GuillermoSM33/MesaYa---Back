using System.ComponentModel.DataAnnotations.Schema;

namespace MesaYa.Models
{
    public class ItemAsRestaurante
    {
        [ForeignKey("Restaurante")]
        public int RestauranteId { get; set; }
        public Restaurante Restaurante { get; set; }
        [ForeignKey("Item")]

        public int ItemId { get; set; }
        public MenuItem Item { get; set; }
    }
}
