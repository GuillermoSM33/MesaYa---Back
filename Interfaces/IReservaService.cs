using MesaYa.DTOs;
using MesaYa.Models;

namespace MesaYa.Interfaces
{
    public interface IReservaService
    {
        Task<Reserva> CrearReservaAsync(CrearReservaDTO dto);
        public Reserva CreateReservaConMultiplesMesas(CrearReservaMultiplesMesasDTO dto);

       List<string> ObtenerHorasDisponibles(int mesaId, DateTime fecha);
        Task<Reserva> ConfirmarReservaAsync(int reservaId);

        Task<Reserva> FinalizarReservaAsync(int reservaId);
        Task<Reserva> CancelarReservaAsync(int reservaId);

        Task<List<ReservaByRestauranteDTO>> GetReservasByRestauranteAsync(int restauranteId);
    }
}
