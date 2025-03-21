using MesaYa.DTOs;
using MesaYa.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MesaYa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MesaController : Controller
    {

        private readonly IMesaService _mesaService;

        public MesaController(IMesaService mesaService)
        {
            _mesaService = mesaService;
        }

        [HttpGet]
        public IActionResult GetMesas()
        {
            try
            {
                var mesas = _mesaService.GetMesas();
                return Ok(
                mesas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("crear")]
        public async Task<IActionResult> CrearMesa([FromBody] CrearMesaDTO dto)
        {
            var resultado = await _mesaService.CreateMesa(dto);
            return Ok(resultado);
        }

        [HttpPatch("soft-delete/{id}")]
        public async Task<IActionResult> SoftDeleteMesa(int id)
        {
            try
            {
                bool result = await _mesaService.SoftDeleteMesa(id);
                if (result)
                    return Ok(new { message = "Mesa eliminada correctamente." });

                return BadRequest(new { message = "No se pudo eliminar la mesa." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> RestoreMesa(int id)
        {
            try
            {
                bool result = await _mesaService.RestoreMesa(id);
                if (result)
                    return Ok(new { message = "Mesa restaurada correctamente." });

                return BadRequest(new { message = "No se pudo restaurar la mesa." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMesa(int id, [FromBody] UpdateMesaDTO updateMesaDTO)
        {
            if (!ModelState.IsValid)  // Validar el DTO
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _mesaService.UpdateMesa(id, updateMesaDTO);
                return Ok(result);  // Devuelve el DTO actualizado
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);  // Devuelve un 404 si la mesa no existe
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);  // Devuelve un 500 en caso de error inesperado
            }
        }

        [HttpGet("restaurante/{restauranteId}")]
        public async Task<IActionResult> GetMesasPorRestaurante(int restauranteId)
        {
            var mesas = await _mesaService.GetMesasPorRestauranteId(restauranteId);
            return Ok(mesas);
        }

        [HttpGet("activas/{restauranteId}")]
        public async Task<IActionResult> GetMesasActivas(int restauranteId)
        {
            var mesas = await _mesaService.GetMesasActivasPorRestauranteId(restauranteId);
            return Ok(mesas);
        }

    }
}
