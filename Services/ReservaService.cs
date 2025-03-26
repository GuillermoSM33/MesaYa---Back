using MesaYa.Data;
using MesaYa.Hubs;
using MesaYa.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MesaYa.Models
{
    public class ReservaService : IReservaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ReservaHub> _hubContext;
        private readonly ValidadorHorariosReserva _validadorHorarios;

        public ReservaService(ApplicationDbContext context, IHubContext<ReservaHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            _validadorHorarios = new ValidadorHorariosReserva(context);
        }

        public Reserva CreateReserva(CrearReservaDTO reservaDTO)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var mesa = _context.Mesa
                    .Include(m => m.Restaurante)
                    .FirstOrDefault(m => m.MesaId == reservaDTO.MesaId && !m.IsDeleted)
                    ?? throw new KeyNotFoundException("La mesa no existe o ha sido eliminada.");

                if (!_context.Usuarios.Any(u => u.UsuarioId == reservaDTO.UsuarioId && !u.IsDeleted))
                    throw new KeyNotFoundException("El usuario no existe o ha sido eliminado.");

                var fechaHoraInicio = reservaDTO.FechaReserva.Date.Add(reservaDTO.HoraReserva);
                var duracion = TimeSpan.FromHours(2); // O obtener de configuración
                var fechaHoraFin = fechaHoraInicio.Add(duracion);

                var reserva = new Reserva
                {
                    UsuarioId = reservaDTO.UsuarioId,
                    FechaReserva = reservaDTO.FechaReserva.Date,
                    HoraInicio = fechaHoraInicio,
                    HoraFin = fechaHoraFin,
                    Estado = "Pendiente",
                    NumeroPersonas = reservaDTO.NumeroPersonas,
                    IsDeleted = false
                };

                // Validaciones
                if (fechaHoraInicio < DateTime.Now)
                    throw new InvalidOperationException("La fecha de reserva no puede ser en el pasado.");

                _validadorHorarios.Validar(reserva, mesa);

                // Persistencia
                _context.Reservas.Add(reserva);
                _context.SaveChanges();

                _context.ReservaAsMesas.Add(new ReservaAsMesa
                {
                    ReservaId = reserva.ReservaId,
                    MesaId = mesa.MesaId
                });

                _context.SaveChanges();
                transaction.Commit();

                // Notificación en tiempo real
                _hubContext.Clients.All.SendAsync("NuevaReservaCreada", reserva.ReservaId);

                return reserva;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Error al crear la reserva.", ex);
            }
        }

        public Reserva CreateReservaConMultiplesMesas(CrearReservaMultiplesMesasDTO dto)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                // Validar usuario
                if (!_context.Usuarios.Any(u => u.UsuarioId == dto.UsuarioId && !u.IsDeleted))
                    throw new KeyNotFoundException("El usuario no existe o ha sido eliminado.");

                // Validar mesas
                var mesas = _context.Mesa
                    .Where(m => dto.MesasIds.Contains(m.MesaId) && !m.IsDeleted)
                    .ToList();

                if (mesas.Count != dto.MesasIds.Count)
                    throw new KeyNotFoundException("Una o más mesas no existen o han sido eliminadas.");

                // Crear reserva
                var fechaHoraInicio = dto.FechaReserva.Date.Add(dto.HoraReserva);
                var duracion = TimeSpan.FromHours(2);
                var fechaHoraFin = fechaHoraInicio.Add(duracion);

                var reserva = new Reserva
                {
                    UsuarioId = dto.UsuarioId,
                    FechaReserva = dto.FechaReserva.Date,
                    HoraInicio = fechaHoraInicio,
                    HoraFin = fechaHoraFin,
                    Estado = "Pendiente",
                    NumeroPersonas = dto.NumeroPersonas,
                    IsDeleted = false
                };

                _context.Reservas.Add(reserva);
                _context.SaveChanges();

                // Asignar mesas y validar disponibilidad
                foreach (var mesa in mesas)
                {
                    _validadorHorarios.Validar(reserva, mesa);

                    _context.ReservaAsMesas.Add(new ReservaAsMesa
                    {
                        ReservaId = reserva.ReservaId,
                        MesaId = mesa.MesaId
                    });

                    mesa.Disponible = false;
                    _context.Mesa.Update(mesa);
                }

                _context.SaveChanges();
                transaction.Commit();

                _hubContext.Clients.All.SendAsync("ReservaActualizada", reserva.ReservaId);

                return reserva;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error al crear reserva: {ex.Message}");
                throw new Exception("Error al crear la reserva con múltiples mesas.", ex);
            }
        }

        public List<string> ObtenerHorasDisponibles(int mesaId, DateTime fecha)
        {
            var mesa = _context.Mesa
                .Include(m => m.Restaurante)
                .FirstOrDefault(m => m.MesaId == mesaId && !m.IsDeleted)
                ?? throw new KeyNotFoundException("La mesa no existe.");

            var restaurante = mesa.Restaurante
                ?? throw new InvalidOperationException("El restaurante asociado no existe.");

            // Obtener reservas existentes para esa mesa en la fecha especificada
            var reservas = _context.ReservaAsMesas
                .Include(rm => rm.Reserva)
                .Where(rm => rm.MesaId == mesaId &&
                             !rm.Reserva.IsDeleted &&
                              rm.Reserva.FechaReserva.Date == fecha.Date)
                .ToList();

            var horasDisponibles = new List<string>();
            var intervalo = TimeSpan.FromMinutes(30); // Intervalo entre reservas

            for (var hora = restaurante.HoraApertura; hora < restaurante.HoraCierre; hora = hora.Add(intervalo))
            {
                var horaFin = hora.Add(TimeSpan.FromHours(2));

                var conflicto = reservas.Any(r =>
                    (hora >= r.Reserva.HoraInicio.TimeOfDay && hora < r.Reserva.HoraFin.TimeOfDay) ||
                    (horaFin > r.Reserva.HoraInicio.TimeOfDay && horaFin <= r.Reserva.HoraFin.TimeOfDay) ||
                    (hora <= r.Reserva.HoraInicio.TimeOfDay && horaFin >= r.Reserva.HoraFin.TimeOfDay));

                if (!conflicto)
                {
                    horasDisponibles.Add(hora.ToString(@"hh\:mm"));
                }
            }

            return horasDisponibles;
        }

        public async Task<Reserva> AceptarReserva(int reservaId)
        {
            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(r => r.ReservaId == reservaId && !r.IsDeleted)
                ?? throw new KeyNotFoundException("La reserva no existe o ha sido cancelada.");

            if (reserva.Estado == "Confirmada")
                throw new InvalidOperationException("La reserva ya está confirmada.");

            reserva.Estado = "Confirmada";
            _context.Reservas.Update(reserva);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReservaConfirmada", reservaId);

            return reserva;
        }

        public async Task CancelarReserva(int reservaId)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var reserva = await _context.Reservas
                    .Include(r => r.ReservaAsMesas)
                    .FirstOrDefaultAsync(r => r.ReservaId == reservaId && !r.IsDeleted)
                    ?? throw new KeyNotFoundException("La reserva no existe o ya ha sido cancelada.");

                reserva.IsDeleted = true;
                reserva.Estado = "Cancelada";
                _context.Reservas.Update(reserva);

                // Liberar mesas
                foreach (var reservaAsMesa in reserva.ReservaAsMesas)
                {
                    var mesa = await _context.Mesa.FindAsync(reservaAsMesa.MesaId);
                    if (mesa != null)
                    {
                        mesa.Disponible = true;
                        _context.Mesa.Update(mesa);
                    }
                }

                await _context.SaveChangesAsync();
                transaction.Commit();

                await _hubContext.Clients.All.SendAsync("ReservaCancelada", reservaId);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Error al cancelar la reserva.", ex);
            }
        }
    }

    public class ValidadorHorariosReserva
    {
        private readonly ApplicationDbContext _context;

        public ValidadorHorariosReserva(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Validar(Reserva reserva, Mesa mesa)
        {
            ValidarCapacidad(reserva, mesa);
            ValidarHorarioRestaurante(reserva, mesa.Restaurante);
            ValidarDisponibilidadMesa(reserva, mesa);
        }

        private void ValidarCapacidad(Reserva reserva, Mesa mesa)
        {
            if (reserva.NumeroPersonas > mesa.Capacidad)
                throw new InvalidOperationException($"La mesa solo tiene capacidad para {mesa.Capacidad} personas.");
        }

        private void ValidarHorarioRestaurante(Reserva reserva, Restaurante restaurante)
        {
            if (restaurante == null)
                throw new InvalidOperationException("No se encontró información del restaurante.");

            if (reserva.HoraInicio.TimeOfDay < restaurante.HoraApertura ||
                reserva.HoraFin.TimeOfDay > restaurante.HoraCierre)
            {
                throw new InvalidOperationException(
                    $"El restaurante solo permite reservas entre {restaurante.HoraApertura:hh\\:mm} y {restaurante.HoraCierre:hh\\:mm}");
            }
        }

        private void ValidarDisponibilidadMesa(Reserva reserva, Mesa mesa)
        {
            var existeReserva = _context.ReservaAsMesas
                .Include(rm => rm.Reserva)
                .Any(rm => rm.MesaId == mesa.MesaId &&
                           !rm.Reserva.IsDeleted &&
                            rm.Reserva.FechaReserva.Date == reserva.FechaReserva.Date &&
                            rm.Reserva.HoraInicio < reserva.HoraFin &&
                            rm.Reserva.HoraFin > reserva.HoraInicio);

            if (existeReserva)
                throw new InvalidOperationException("La mesa ya está reservada en ese horario.");
        }
    }
}