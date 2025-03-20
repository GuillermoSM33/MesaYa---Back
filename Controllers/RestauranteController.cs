
using MesaYa.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MesaYa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestauranteController : Controller
    {
        private readonly IRestauranteService _restauranteService;

        public RestauranteController(IRestauranteService restauranteService)
        {
            _restauranteService = restauranteService;
        }


        [HttpGet]
        public IActionResult GetRestaurante()
        {
            try
            {
                var restaurantes = _restauranteService.GetRestaurante();
                return Ok(
                restaurantes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("onlytrue")]
        public IActionResult GetRestaurantesByTrue()
        {
            try
            {
                var restaurantes = _restauranteService.GetRestaurantesByTrue();
                return Ok(
                restaurantes);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestauranteById(int id)
        {
            try
            {
                var restaurante = await _restauranteService.GetRestauranteById(id);
                return Ok(restaurante);
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


        [HttpPost]
        public async Task<IActionResult> CreateRestaurante([FromBody] CrearRestauranteDTO crearRestauranteDTO)
        {
            try
            {
                var restaurante = await _restauranteService.CreateRestaurante(crearRestauranteDTO);
                return Ok(restaurante);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurante(int id, [FromBody] UpdateRestauranteDTO updateRestauranteDTO)
        {
            try
            {
                var restaurante = await _restauranteService.UpdateRestaurante(id, updateRestauranteDTO);
                return Ok(restaurante);
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


        [HttpPatch("restore/{id}")]
        public async Task<IActionResult> SoftRestoreRestaurante(int id)
        {
            try
            {
                var result = await _restauranteService.SoftRestoreRestaurante(id);
                return Ok(result);  // Devuelve true si se restauró correctamente
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);  // Devuelve un 404 si el restaurante no existe
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);  // Devuelve un 500 en caso de error inesperado
            }
        }


        [HttpPatch("soft-delete/{id}")]
        public async Task<IActionResult> SoftDeleteRestaurante(int id)
        {
            try
            {
                var result = await _restauranteService.SoftDeleteRestaurante(id);
                return Ok(result);  // Devuelve true si se marcó como eliminado correctamente
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);  // Devuelve un 404 si el restaurante no existe
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);  // Devuelve un 500 en caso de error inesperado
            }
        }



    }
}
