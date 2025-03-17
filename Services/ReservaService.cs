
using MesaYa.Data;
using Microsoft.EntityFrameworkCore;

namespace MesaYa.Models
{
    public class ReservaService : IReservaService
    {
        private readonly ApplicationDbContext _context;

        public ReservaService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Reserva CreateReserva(CrearReservaDTO reservaDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Validar que la mesa exista
                    var mesa = _context.Mesa
                        .FirstOrDefault(m => m.MesaId == reservaDTO.MesaId && !m.IsDeleted);

                    if (mesa == null)
                    {
                        throw new KeyNotFoundException("La mesa no existe o ha sido eliminada.");
                    }

                    // Validar que el usuario exista
                    var usuarioExistente = _context.Usuarios.Any(u => u.UsuarioId == reservaDTO.UsuarioId && !u.IsDeleted);
                    if (!usuarioExistente)
                    {
                        throw new KeyNotFoundException("El usuario no existe o ha sido eliminado.");
                    }

                    // Validar que la fecha de reserva no sea en el pasado
                    if (reservaDTO.FechaReserva < DateTime.Now)
                    {
                        throw new InvalidOperationException("La fecha de reserva no puede ser en el pasado.");
                    }

                    // Validar que el número de personas no exceda la capacidad de la mesa
                    if (reservaDTO.NumeroPersonas > mesa.Capacidad)
                    {
                        throw new InvalidOperationException($"La mesa solo tiene capacidad para {mesa.Capacidad} personas.");
                    }

                    // Verificar si el usuario ya tiene una reserva pendiente
                    var reservaPendiente = _context.Reservas
                        .Any(r => r.UsuarioId == reservaDTO.UsuarioId && r.Estado == "Pendiente" && !r.IsDeleted);

                    if (reservaPendiente)
                    {
                        throw new InvalidOperationException("El usuario ya tiene una reserva pendiente. No se puede crear otra reserva.");
                    }

                    // Crear la nueva reserva
                    var nuevaReserva = new Reserva
                    {
                        MesaId = reservaDTO.MesaId,
                        UsuarioId = reservaDTO.UsuarioId,
                        FechaReserva = reservaDTO.FechaReserva,
                        Estado = "Pendiente",
                        NumeroPersonas = reservaDTO.NumeroPersonas,
                        IsDeleted = false,
                    };

                    // Agregar la reserva al contexto
                    _context.Reservas.Add(nuevaReserva);

                    // Guardar los cambios en la base de datos para obtener el ID de la reserva
                    _context.SaveChanges();

                    // Cambiar el estado de la mesa a "No disponible"
                    mesa.Disponible = false;  // Actualizar el campo Disponible de la mesa
                    _context.Mesa.Update(mesa);  // Marcar la mesa como modificada

                    // Crear la relación en la tabla pivote (ReservaAsMesa)
                    var reservaAsMesa = new ReservaAsMesa
                    {
                        ReservaId = nuevaReserva.ReservaId,  // Usar el ID de la reserva recién creada
                        MesaId = mesa.MesaId,               // Usar el ID de la mesa
                    };

                    _context.ReservaAsMesas.Add(reservaAsMesa);

                    // Guardar los cambios nuevamente para persistir la relación y el estado de la mesa
                    _context.SaveChanges();

                    // Confirmar la transacción
                    transaction.Commit();

                    return nuevaReserva;
                }
                catch (Exception ex)
                {
                    // Revertir la transacción en caso de error
                    transaction.Rollback();
                    throw new Exception("Error al crear la reserva.", ex);
                }
            }
        }

        public void CancelarReserva(int reservaId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Buscar la reserva por su ID
                    var reserva = _context.Reservas
                        .Include(r => r.ReservaAsMesas)
                        .FirstOrDefault(r => r.ReservaId == reservaId && !r.IsDeleted);

                    if (reserva == null)
                    {
                        throw new KeyNotFoundException("La reserva no existe o ya ha sido cancelada.");
                    }

                    // Cambiar el estado de la reserva a eliminada
                    reserva.IsDeleted = true;
                    reserva.Estado = "Cancelada";
                    _context.Reservas.Update(reserva);

                    // Cambiar el estado de la mesa a disponible
                    var mesa = _context.Mesa.FirstOrDefault(m => m.MesaId == reserva.MesaId);
                    if (mesa != null)
                    {
                        mesa.Disponible = true;
                        _context.Mesa.Update(mesa);
                    }

                    // Eliminar la relación en la tabla pivote (ReservaAsMesa)
                    var reservaAsMesa = reserva.ReservaAsMesas.FirstOrDefault();
                    if (reservaAsMesa != null)
                    {
                        _context.ReservaAsMesas.Remove(reservaAsMesa);
                    }

                    // Guardar los cambios en la base de datos
                    _context.SaveChanges();

                    // Confirmar la transacción
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Revertir la transacción en caso de error
                    transaction.Rollback();
                    throw new Exception("Error al cancelar la reserva.", ex);
                }
            }
        }










    }
}