using MesaYa.Models;

    using MesaYa.Interfaces;

public interface IReservaService
{
    Reserva CreateReserva(CrearReservaDTO reservaDTO);

    void CancelarReserva(int reservaId);

}
