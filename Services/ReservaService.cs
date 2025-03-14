
using MesaYa.Data;

namespace MesaYa.Models
{
    public class ReservaService : IReservaService
    {
        private readonly ApplicationDbContext _context;

        public ReservaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Reserva CreateReserva(Reserva reserva)
        {
            // Verificar si el usuario ya tiene una reserva pendiente
            var reservaPendiente = _context.Reservas
                .Any(r => r.UsuarioId == reserva.UsuarioId && r.Estado == "Pendiente" && !r.IsDeleted);

            if (reservaPendiente)
            {
                throw new InvalidOperationException("El usuario ya tiene una reserva pendiente. No se puede crear otra reserva.");
            }

            // Crear la nueva reserva
            var nuevaReserva = new Reserva
            {
                UsuarioId = reserva.UsuarioId,
                FechaReserva = reserva.FechaReserva,
                Estado = "Pendiente", 
                NumeroPersonas = reserva.NumeroPersonas,
                IsDeleted = false,
            };

            _context.Reservas.Add(nuevaReserva);
            _context.SaveChanges();

            return nuevaReserva;
        }
    }
}