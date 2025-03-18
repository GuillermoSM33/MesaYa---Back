using MesaYa.Interfaces;
using MesaYa.Models;
using Microsoft.AspNetCore.Mvc;

namespace MesaYa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }


        [HttpPost]
        public IActionResult CreateReserva([FromBody] CrearReservaDTO crearReservaDTO)
        {
            if (!ModelState.IsValid)  // Validar el DTO
            {
                return BadRequest(ModelState);
            }

            try
            {
                var reserva = _reservaService.CreateReserva(crearReservaDTO);
                return Ok(reserva);  // Devuelve la reserva creada
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);  // Devuelve un 404 si la mesa o el usuario no existen
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);  // Devuelve un 400 si la reserva no es válida
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);  // Devuelve un 500 en caso de error inesperado
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


    }
}
