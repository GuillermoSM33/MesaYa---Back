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
        public async Task<IActionResult> CrearReservaAsync([FromBody] CrearReservaDTO crearReservaDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                Console.WriteLine("Paso 1: Iniciando creación");

                var reserva = await _reservaService.CrearReservaAsync(crearReservaDTO);

                Console.WriteLine("Paso 2: Reserva creada");

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == reserva.UsuarioId);
                if (usuario == null)
                    return NotFound("Usuario no encontrado para la reserva.");

                var nombreUsuario = usuario.Username;

                var mesaId = reserva.ReservaAsMesas?.FirstOrDefault()?.MesaId ?? 0;
                Console.WriteLine($"Paso 3: MesaId = {mesaId}");

                var qrContenido = $"ReservaId:{reserva.ReservaId}|Usuario:{nombreUsuario}|Fecha:{reserva.FechaReserva:yyyy-MM-dd HH:mm}|Fecha final de la reseserva:{reserva.HoraFin:yyyy-MM-dd HH:mm}|MesaId:{mesaId}|Personas:{reserva.NumeroPersonas}";

                var qrBytes = QrService.GenerarQr(qrContenido);
                var pdfDoc = new ReservaPdf(qrContenido, qrBytes, nombreUsuario);
                var pdfBytes = pdfDoc.GeneratePdf();

                var mensaje = $"Tu reserva ha sido exitosa.<br>" +
                              $"Fecha: {reserva.FechaReserva}<br>" +
                              $"Horario de finalización {reserva.HoraFin}<br>" +
                              $"Capacidad: {reserva.NumeroPersonas} personas<br>" +
                              $"Estado: {reserva.Estado}";

                var notificacion = await _notificacionService.CrearNotificacionAsync(reserva.UsuarioId, mensaje, "Reserva");

                await _notificacionService.EnviarNotificacionAsync(notificacion, pdfBytes, $"reserva_{reserva.ReservaId}.pdf");

                Console.WriteLine("Paso final: OK");

                return Ok(reserva);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR INTERNO: " + ex.Message);
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

                // Obtener nombre del usuario
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.UsuarioId == reserva.UsuarioId);

                if (usuario == null)
                    return NotFound("Usuario no encontrado para la reserva.");

                string nombreUsuario = usuario.Username;

                // Obtener mesaId (de la primera mesa asociada)
                var mesaId = reserva.ReservaAsMesas.FirstOrDefault()?.MesaId ?? 0;

                // Mensaje del correo
                var mensaje = $"Tu reserva ha sido <strong>confirmada</strong>.<br>" +
                              $"Fecha: {reserva.FechaReserva:dd/MM/yyyy HH:mm}<br>" +
                              $"Capacidad: {reserva.NumeroPersonas} personas<br>" +
                              $"Estado: {reserva.Estado}<br>" +
                              $"Tienes 15 minutos de tolerancia, después de eso la reserva podrá ser cancelada automáticamente.";

                // Contenido del QR
                var qrContenido = $"ReservaId:{reserva.ReservaId}|Usuario:{nombreUsuario}|Fecha:{reserva.FechaReserva:yyyy-MM-dd HH:mm}|FechaFin:{reserva.HoraFin:yyyy-MM-dd HH:mm}|MesaId:{mesaId}|Personas:{reserva.NumeroPersonas}";

                // Generar QR
                var qrBytes = QrService.GenerarQr(qrContenido);

                // Generar PDF con aviso de tolerancia
                var pdfMensaje = $"Reserva Confirmada\n\n" +
                                 $"Usuario: {nombreUsuario}\n" +
                                 $"Reserva ID: {reserva.ReservaId}\n" +
                                 $"Fecha: {reserva.FechaReserva:dd/MM/yyyy HH:mm}\n" +
                                 $"Hora Fin: {reserva.HoraFin:HH:mm}\n" +
                                 $"Mesa: {mesaId}\n" +
                                 $"Personas: {reserva.NumeroPersonas}\n\n" +
                                 $"**Tienes 15 minutos de tolerancia, de lo contrario se cancelará la reserva.**";

                var pdfDoc = new ReservaPdf(pdfMensaje, qrBytes, nombreUsuario);
                var pdfBytes = pdfDoc.GeneratePdf();

                // Crear y enviar notificación
                var notificacion = await _notificacionService.CrearNotificacionAsync(reserva.UsuarioId, mensaje, "Reserva Confirmada");
                await _notificacionService.EnviarNotificacionAsync(notificacion, pdfBytes, $"reserva_confirmada_{reserva.ReservaId}.pdf");

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

        [HttpPut("finalizar/{reservaId}")]
        public async Task<IActionResult> FinalizarReserva(int reservaId)
        {
            try
            {
                var reserva = await _reservaService.FinalizarReservaAsync(reservaId);

                return Ok(new { mensaje = "Reserva finalizada exitosamente.", reserva });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }




        [HttpPut("cancelar/{reservaId}")]
        public async Task<IActionResult> CancelarReserva(int reservaId)
        {
            try
            {
                var reserva = await _reservaService.CancelarReservaAsync(reservaId);

                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.UsuarioId == reserva.UsuarioId);
                if (usuario == null)
                    return NotFound("Usuario no encontrado.");

                var nombreUsuario = usuario.Username;

                // Construir mensaje
                var mensaje = $"Tu reserva ha sido <strong>cancelada</strong>.<br>" +
                              $"Fecha original: {reserva.FechaReserva:dd/MM/yyyy HH:mm}<br>" +
                              $"Capacidad: {reserva.NumeroPersonas} personas<br>" +
                              $"Estado: {reserva.Estado}";

                // Generar contenido del QR
                var qrContenido = $"ReservaId:{reserva.ReservaId}|Usuario:{nombreUsuario}|Fecha:{reserva.FechaReserva:yyyy-MM-dd HH:mm}|Estado:Cancelada";

                var qrBytes = QrService.GenerarQr(qrContenido);

                // Generar PDF con el mensaje de cancelación
                var pdfMensaje = $"Reserva Cancelada\n\n" +
                                 $"Usuario: {nombreUsuario}\n" +
                                 $"Reserva ID: {reserva.ReservaId}\n" +
                                 $"Fecha original: {reserva.FechaReserva:dd/MM/yyyy HH:mm}\n" +
                                 $"Personas: {reserva.NumeroPersonas}\n\n" +
                                 $"Si tienes dudas, comunícate con el restaurante.";

                var pdfDoc = new ReservaPdf(pdfMensaje, qrBytes, nombreUsuario);
                var pdfBytes = pdfDoc.GeneratePdf();

                // Crear notificación y enviar
                var notificacion = await _notificacionService.CrearNotificacionAsync(reserva.UsuarioId, mensaje, "Reserva Cancelada");
                await _notificacionService.EnviarNotificacionAsync(notificacion, pdfBytes, $"reserva_cancelada_{reserva.ReservaId}.pdf");

                return Ok(new { mensaje = "Reserva cancelada y correo enviado.", reserva });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
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

        [HttpGet("restaurante/{restauranteId}")]
        public async Task<IActionResult> GetReservasByRestaurante(int restauranteId)
        {
            try
            {
                var reservas = await _reservaService.GetReservasByRestauranteAsync(restauranteId);
                return Ok(reservas);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
