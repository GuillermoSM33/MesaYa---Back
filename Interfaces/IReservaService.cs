using MesaYa.Models;

namespace MesaYa.Interfaces
{
    public interface IReservaService
    {
        Task<Reserva> CrearReservaAsync(CrearReservaDTO dto);
        public Reserva CreateReservaConMultiplesMesas(CrearReservaMultiplesMesasDTO dto);

       List<string> ObtenerHorasDisponibles(int mesaId, DateTime fecha);
        Task<Reserva> ConfirmarReservaAsync(int reservaId);

        void CancelarReserva(int reservaId);
    }
}
