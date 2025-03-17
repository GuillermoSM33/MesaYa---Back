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


        [HttpPost]
        public IActionResult CreateMesa([FromBody] CrearMesaDTO crearMesaDTO)
        {
            try
            {
                var mesa = _mesaService.CreateMesa(crearMesaDTO);
                return Ok(mesa);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
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



    }
}
