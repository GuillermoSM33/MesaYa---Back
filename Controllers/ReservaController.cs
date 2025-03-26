using MesaYa.Interfaces;
using MesaYa.Models;
using MesaYa.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MesaYa.Document;
using MesaYa.Services;
using QuestPDF.Fluent;
using MesaYa.Data;
using Microsoft.EntityFrameworkCore;

namespace MesaYa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;
        private readonly INotificacionService _notificacionService;
        private readonly ApplicationDbContext _context;

        public ReservaController(IReservaService reservaService, INotificacionService notificacionService, ApplicationDbContext context)
        {
            _reservaService = reservaService;
            _notificacionService = notificacionService;
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> CreateReserva([FromBody] CrearReservaDTO crearReservaDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var reserva = await _reservaService.CrearReservaAsync(crearReservaDTO);

                var mensaje = $"Tu reserva ha sido exitosa.<br>" +
                              $"Fecha: {reserva.FechaReserva}<br>" +
                              $"Capacidad: {reserva.NumeroPersonas} personas<br>" +
                              $"Estado: {reserva.Estado}";

                var notificacion = await _notificacionService.CrearNotificacionAsync(reserva.UsuarioId, mensaje, "Reserva");

                await _notificacionService.EnviarNotificacionAsync(notificacion);

                return Ok(reserva);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("crear-multiples-mesas")]
        public async Task<IActionResult> CrearReservaConMultiplesMesas([FromBody] CrearReservaMultiplesMesasDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var reserva = _reservaService.CreateReservaConMultiplesMesas(dto);
                return Ok(reserva);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("confirmar/{reservaId}")]
        public async Task<IActionResult> ConfirmarReserva(int reservaId)
        {
            try
            {
                var reserva = await _reservaService.ConfirmarReservaAsync(reservaId);

                var mensaje = $"Tu reserva ha sido confirmada.<br>" +
                              $"Fecha: {reserva.FechaReserva}<br>" +
                              $"Capacidad: {reserva.NumeroPersonas} personas<br>" +
                              $"Estado: {reserva.Estado}";

                var notificacion = await _notificacionService.CrearNotificacionAsync(reserva.UsuarioId, mensaje, "Reserva confirmada");

                await _notificacionService.EnviarNotificacionAsync(notificacion);

                return Ok(reserva);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }




        [HttpPatch("{reservaId}/cancelar")]
        public IActionResult CancelarReserva(int reservaId)
        {
            try
            {
                _reservaService.CancelarReserva(reservaId);  // Llama al servicio para cancelar la reserva
                return Ok(new { Message = "Reserva cancelada exitosamente." });  // Devuelve un mensaje de éxito
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);  // Devuelve un 404 si la reserva no existe o ya está cancelada
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);  // Devuelve un 500 en caso de error inesperado
            }
        }
        [HttpGet("disponibilidad")]
        public IActionResult ObtenerHorasDisponibles([FromQuery] int mesaId, [FromQuery] DateTime fecha)
        {
            try
            {
                var horas = _reservaService.ObtenerHorasDisponibles(mesaId, fecha);
                return Ok(horas);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al obtener horas disponibles: " + ex.Message);
            }
        }

    }
}
