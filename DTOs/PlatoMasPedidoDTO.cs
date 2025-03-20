namespace MesaYa.DTOs
{
    public class PlatoMasPedidoDTO
    {
        public int ItemId { get; set; }
        public string Nombre { get; set; }
        public string RestauranteNombre { get; set; } // Nuevo campo
        public string CategoriaNombre { get; set; } // Nuevo campo
        public int TotalPedidos { get; set; }
    }
}
