namespace MesaYa.DTOs
{
    public class ReservaByRestauranteDTO
    {
       
            public int ReservaId { get; set; }
            public int RestauranteId { get; set; }
            public string Estado { get; set; }

            public int UsuarioId { get; set; }
            public string UsuarioNombre { get; set; }

            public int NumeroPersonas { get; set; }

            public List<int> MesaIds { get; set; }
            public List<int> MesaNumeros { get; set; }

            public DateTime FechaReserva { get; set; }
            public DateTime HoraFin { get; set; }

            public bool IsDeleted { get; set; }
    
    }
}
