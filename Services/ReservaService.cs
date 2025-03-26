using MesaYa.Data;
using MesaYa.DTOs;
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
        //private readonly ValidadorHorariosReserva _validadorHorarios;
        
        public ReservaService(ApplicationDbContext context, IHubContext<ReservaHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
           //  _validadorHorarios = new ValidadorHorariosReserva(context);
        }


        public async Task<Reserva> CrearReservaAsync(CrearReservaDTO dto)
        {
            var mesa = await _context.Mesa
                .Include(m => m.Restaurante)
                .FirstOrDefaultAsync(m => m.MesaId == dto.MesaId && !m.IsDeleted);

            if (mesa == null)
                throw new KeyNotFoundException("La mesa no existe.");

            if (mesa.Restaurante == null)
                throw new InvalidOperationException("El restaurante asociado no existe.");

            var horaReserva = dto.FechaReserva.TimeOfDay;

            if (horaReserva < mesa.Restaurante.HoraApertura || horaReserva >= mesa.Restaurante.HoraCierre)
                throw new InvalidOperationException("La hora solicitada está fuera del horario del restaurante.");

            var horaFin = dto.FechaReserva.AddHours(2);

            var reservas = await _context.ReservaAsMesas
                .Include(rm => rm.Reserva)
                .Where(rm => rm.MesaId == dto.MesaId &&
                             !rm.Reserva.IsDeleted &&
                             rm.Reserva.FechaReserva.Date == dto.FechaReserva.Date)
                .ToListAsync();

            bool conflicto = reservas.Any(r =>
                (dto.FechaReserva >= r.Reserva.FechaReserva && dto.FechaReserva < r.Reserva.HoraFin) ||
                (horaFin > r.Reserva.FechaReserva && horaFin <= r.Reserva.HoraFin) ||
                (dto.FechaReserva <= r.Reserva.FechaReserva && horaFin >= r.Reserva.HoraFin));

            if (conflicto)
                throw new InvalidOperationException("La hora seleccionada ya está reservada para esta mesa.");

            var reserva = new Reserva
            {
                UsuarioId = dto.UsuarioId,
                FechaReserva = dto.FechaReserva,
                HoraFin = horaFin,
                NumeroPersonas = dto.NumeroPersonas,
                Estado = "Pendiente",
                IsDeleted = false
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            var reservaMesa = new ReservaAsMesa
            {
                MesaId = dto.MesaId,
                ReservaId = reserva.ReservaId
            };

            _context.ReservaAsMesas.Add(reservaMesa);
            await _context.SaveChangesAsync();

            return reserva;
        }



        public async Task<Reserva> ConfirmarReservaAsync(int reservaId)
        {
            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(r => r.ReservaId == reservaId && !r.IsDeleted);

            if (reserva == null)
                throw new KeyNotFoundException("La reserva no existe.");

            if (reserva.Estado == "Confirmada")
                throw new InvalidOperationException("La reserva ya está confirmada.");

            reserva.Estado = "Confirmada";
            _context.Reservas.Update(reserva);
            await _context.SaveChangesAsync();

            return reserva;
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
                .Select(rm => rm.Reserva)
                .ToList();

            var horasDisponibles = new List<string>();
            var intervalo = TimeSpan.FromMinutes(30); // Intervalos de media hora

            // Convertir la fecha recibida y las horas de apertura/cierre en DateTime completos
            var horaApertura = fecha.Date + restaurante.HoraApertura;
            var horaCierre = fecha.Date + restaurante.HoraCierre;

            for (var hora = horaApertura; hora.AddHours(2) <= horaCierre; hora = hora.Add(intervalo))
            {
                var horaFin = hora.AddHours(2);

                // Verificar si se cruza con alguna reserva existente
                var conflicto = reservas.Any(r =>
                    (hora >= r.FechaReserva && hora < r.HoraFin) ||
                    (horaFin > r.FechaReserva && horaFin <= r.HoraFin) ||
                    (hora <= r.FechaReserva && horaFin >= r.HoraFin));

                if (!conflicto)
                {
                    horasDisponibles.Add(hora.ToString("HH:mm"));
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


        public async Task<Reserva> FinalizarReservaAsync(int reservaId)
        {
            var reserva = await _context.Reservas
                .Include(r => r.ReservaAsMesas)
                .FirstOrDefaultAsync(r => r.ReservaId == reservaId && !r.IsDeleted)
                ?? throw new KeyNotFoundException("Reserva no encontrada.");

            reserva.Estado = "Finalizado";

            // Eliminar de tabla pivote
            _context.ReservaAsMesas.RemoveRange(reserva.ReservaAsMesas);

            await _context.SaveChangesAsync();
            return reserva;
        }


        public async Task<List<ReservaByRestauranteDTO>> GetReservasByRestauranteAsync(int restauranteId)
        {
            var reservas = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.ReservaAsMesas)
                    .ThenInclude(rm => rm.Mesa)
                .Where(r => r.ReservaAsMesas.Any(rm => rm.Mesa.RestauranteId == restauranteId))
                .OrderByDescending(r => r.FechaReserva)
                .ToListAsync();

            if (!reservas.Any())
                throw new KeyNotFoundException("No se encontraron reservas para este restaurante.");

            var reservasDTO = reservas.Select(r => new ReservaByRestauranteDTO
            {
                ReservaId = r.ReservaId,
                RestauranteId = r.ReservaAsMesas.First().Mesa.RestauranteId, // todas las mesas son del mismo restaurante
                Estado = r.Estado,
                UsuarioId = r.UsuarioId,
                UsuarioNombre = r.Usuario?.Username ?? "Desconocido",
                NumeroPersonas = r.NumeroPersonas,
                MesaIds = r.ReservaAsMesas.Select(rm => rm.MesaId).ToList(),
                MesaNumeros = r.ReservaAsMesas.Select(rm => rm.Mesa.MesaNumero).ToList(),
                FechaReserva = r.FechaReserva,
                HoraFin = r.HoraFin,
                IsDeleted = r.IsDeleted
            }).ToList();

            return reservasDTO;
        }


        public async Task<Reserva> CancelarReservaAsync(int reservaId)
        {
            var reserva = await _context.Reservas
                .Include(r => r.ReservaAsMesas)
                .FirstOrDefaultAsync(r => r.ReservaId == reservaId && !r.IsDeleted)
                ?? throw new KeyNotFoundException("Reserva no encontrada.");

            reserva.Estado = "Cancelado";

            _context.ReservaAsMesas.RemoveRange(reserva.ReservaAsMesas);

            await _context.SaveChangesAsync();
            return reserva;
        }

    }


}