namespace MesaYa.Models
{
    public class CrearReservaMultiplesMesasDTO
    {
        public int UsuarioId { get; set; }
        public List<int> MesasIds { get; set; }
        public DateTime FechaReserva { get; set; }
        public TimeSpan HoraInicio {get;set;}
        public int NumeroPersonas { get; set; }
    }
}
