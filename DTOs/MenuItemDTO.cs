namespace MesaYa.DTOs
{
    public class MenuItemDTO
    {
        public int Id { get; set; }
        public string NombreItem { get; set; }
        public string Descripcion { get;set; }
        public decimal Precio {  get; set; }

        public string CategoriaNombre { get;set; }

        public string Imagen { get; set; }
        public bool Disponible { get; set; }
        public bool isDeleted { get; set; }
    }
}
