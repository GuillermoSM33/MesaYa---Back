using System.ComponentModel.DataAnnotations.Schema;

namespace MesaYa.Models
{
    
        public class ItemAsRestaurante
        {
            [ForeignKey("Restaurante")]
            public int RestauranteId { get; set; }
            public Restaurante Restaurante { get; set; }

            [ForeignKey("MenuItem")] // Cambiado de "Item" a "MenuItem"
            public int ItemId { get; set; }
            public MenuItem MenuItem { get; set; } // Cambiado de "Item" a "MenuItem"
        }
}
