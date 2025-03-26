

public class CrearReservaDTO
{
    public int MesaId { get; set; }
    public int UsuarioId { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public DateTime FechaReserva { get; set; }
    public int NumeroPersonas { get; set; }
}


