using MesaYa.DTOs;
using MesaYa.Models;
using System.Threading.Tasks;

namespace MesaYa.Interfaces
{
    public interface INotificacionService
    {
        Task<Notificacion> CrearNotificacionAsync(int usuarioId, string mensaje, string tipo);
        Task EnviarNotificacionAsync(Notificacion notificacion);
    }
}


