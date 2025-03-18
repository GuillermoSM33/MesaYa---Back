using MesaYa.Models;

namespace MesaYa.Interfaces
{
    public interface IReservaService
    {
        Reserva CreateReserva(CrearReservaDTO reservaDTO);
        public Reserva CreateReservaConMultiplesMesas(CrearReservaMultiplesMesasDTO dto);
        void CancelarReserva(int reservaId);
    }
}
