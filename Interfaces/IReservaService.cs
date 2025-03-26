using MesaYa.Models;

namespace MesaYa.Interfaces
{
    public interface IReservaService
    {
        Reserva CreateReserva(CrearReservaDTO reservaDTO);
        public Reserva CreateReservaConMultiplesMesas(CrearReservaMultiplesMesasDTO dto);
        List<string> ObtenerHorasDisponibles(int mesaId);
        void CancelarReserva(int reservaId);
    }
}
