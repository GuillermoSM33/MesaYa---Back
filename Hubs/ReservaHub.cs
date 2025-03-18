using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MesaYa.Hubs
{
    public class ReservaHub : Hub
    {
        public async Task NotificarNuevaReserva(int reservaId)
        {
            await Clients.All.SendAsync("ReservaActualizada", reservaId);
        }

        public async Task NotificarCancelacion(int reservaId)
        {
            await Clients.All.SendAsync("ReservaCancelada", reservaId);
        }
    }
}
