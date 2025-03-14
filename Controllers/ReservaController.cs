using MesaYa.Models;
using Microsoft.AspNetCore.Mvc;

namespace MesaYa.Controllers
{
    public class ReservaController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservaController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }


        [HttpPost("reserva")]

        public IActionResult CreateReserva([FromBody] Reserva reserva)
        {
            try
            {
                var nuevaReserva = _reservaService.CreateReserva(reserva);
                return Ok(nuevaReserva);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
