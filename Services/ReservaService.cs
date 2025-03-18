using MesaYa.Data;
using MesaYa.Hubs;
using MesaYa.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace MesaYa.Models
{
    public class ReservaService : IReservaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ReservaHub> _hubContext;

        public ReservaService(ApplicationDbContext context, IHubContext<ReservaHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public Reserva CreateReserva(CrearReservaDTO reservaDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var mesa = _context.Mesa
                        .FirstOrDefault(m => m.MesaId == reservaDTO.MesaId && !m.IsDeleted);

                    if (mesa == null)
                    {
                        throw new KeyNotFoundException("La mesa no existe o ha sido eliminada.");
                    }

                    var usuarioExistente = _context.Usuarios.Any(u => u.UsuarioId == reservaDTO.UsuarioId && !u.IsDeleted);
                    if (!usuarioExistente)
                    {
                        throw new KeyNotFoundException("El usuario no existe o ha sido eliminado.");
                    }

                    if (reservaDTO.FechaReserva < DateTime.Now)
                    {
                        throw new InvalidOperationException("La fecha de reserva no puede ser en el pasado.");
                    }

                    if (reservaDTO.NumeroPersonas > mesa.Capacidad)
                    {
                        throw new InvalidOperationException($"La mesa solo tiene capacidad para {mesa.Capacidad} personas.");
                    }

                    var nuevaReserva = new Reserva
                    {
                        UsuarioId = reservaDTO.UsuarioId,
                        FechaReserva = reservaDTO.FechaReserva,
                        Estado = "Pendiente",
                        NumeroPersonas = reservaDTO.NumeroPersonas,
                        IsDeleted = false,
                    };

                    _context.Reservas.Add(nuevaReserva);
                    _context.SaveChanges(); // Guardar para obtener ReservaId

                    var reservaAsMesa = new ReservaAsMesa
                    {
                        ReservaId = nuevaReserva.ReservaId,
                        MesaId = mesa.MesaId
                    };
                    _context.ReservaAsMesas.Add(reservaAsMesa);

                    mesa.Disponible = false;
                    _context.Mesa.Update(mesa);

                    _context.SaveChanges();
                    transaction.Commit();

                    return nuevaReserva;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error al crear la reserva.", ex);
                }
            }
        }

        public Reserva CreateReservaConMultiplesMesas(CrearReservaMultiplesMesasDTO dto)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var nuevaReserva = new Reserva
                    {
                        UsuarioId = dto.UsuarioId,
                        FechaReserva = dto.FechaReserva,
                        Estado = "Pendiente",
                        NumeroPersonas = dto.NumeroPersonas,
                        IsDeleted = false,
                    };

                    _context.Reservas.Add(nuevaReserva);
                    _context.SaveChanges(); // Guardar para obtener ReservaId

                    foreach (var mesaId in dto.MesasIds)
                    {
                        var mesa = _context.Mesa.FirstOrDefault(m => m.MesaId == mesaId && !m.IsDeleted);
                        if (mesa == null)
                            throw new KeyNotFoundException($"La mesa {mesaId} no existe o ha sido eliminada.");

                        if (!mesa.Disponible)
                            throw new InvalidOperationException($"La mesa {mesaId} ya está reservada.");

                        mesa.Disponible = false;
                        _context.Mesa.Update(mesa);

                        var reservaAsMesa = new ReservaAsMesa
                        {
                            ReservaId = nuevaReserva.ReservaId,
                            MesaId = mesa.MesaId
                        };
                        _context.ReservaAsMesas.Add(reservaAsMesa);
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    _hubContext.Clients.All.SendAsync("ReservaActualizada", nuevaReserva.ReservaId);

                    return nuevaReserva;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"⚠️ Error al crear la reserva: {ex.Message}");
                    Console.WriteLine($"🔍 StackTrace: {ex.StackTrace}");
                    throw new Exception("Error al crear la reserva con múltiples mesas.", ex);
                }
            }
        }

        public void CancelarReserva(int reservaId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var reserva = _context.Reservas
                        .Include(r => r.ReservaAsMesas)
                        .FirstOrDefault(r => r.ReservaId == reservaId && !r.IsDeleted);

                    if (reserva == null)
                    {
                        throw new KeyNotFoundException("La reserva no existe o ya ha sido cancelada.");
                    }

                    reserva.IsDeleted = true;
                    reserva.Estado = "Cancelada";
                    _context.Reservas.Update(reserva);

                    foreach (var reservaAsMesa in reserva.ReservaAsMesas)
                    {
                        var mesa = _context.Mesa.FirstOrDefault(m => m.MesaId == reservaAsMesa.MesaId);
                        if (mesa != null)
                        {
                            mesa.Disponible = true;
                            _context.Mesa.Update(mesa);
                        }
                    }

                    _context.ReservaAsMesas.RemoveRange(reserva.ReservaAsMesas);

                    _context.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error al cancelar la reserva.", ex);
                }
            }
        }
    }
}
