namespace MesaYa.DTOs
{
    public class NotificacionDto
    {
        public int NotificacionId { get; set; }
        public string Mensaje { get; set; }
        public string Tipo { get; set; }
        public bool Enviado { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public DateTime CreatedAt { get; set; }

        public string RestauranteNombre { get; set; }
        public DateTime FechaReserva { get; set; }
        public int NumeroMesas { get; set; }
        public int NumeroPersonas { get; set; }
        public string? NombreUsuario { get; set; } 
    }
}
