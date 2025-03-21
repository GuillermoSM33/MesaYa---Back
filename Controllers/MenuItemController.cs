using MesaYa.DTOs;
using MesaYa.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MesaYa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : Controller
    {
        private readonly IMenuItemService _menuItemService;

        public MenuItemController(IMenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }



        [HttpGet]
        public IActionResult GetMenus()
        {
            try
            {
                var menus = _menuItemService.GetMenus();
                return Ok(menus);
            }
            catch (Exception ex) { 
            return BadRequest(ex.Message);
            }
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearMenuItem([FromBody] CrearMenuItemDTO dto)
        {
            try
            {
                var nuevoItem = await _menuItemService.CreateMenuItem(dto);
                return Ok(nuevoItem);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }


        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarMenuItem(int id, [FromBody] CrearMenuItemDTO dto)
        {
            try
            {
                var menuItemActualizado = await _menuItemService.UpdateMenuItem(id, dto);
                return Ok(menuItemActualizado);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }

        [HttpGet("restaurante/{restauranteId}")]
        public IActionResult GetMenusByRestaurante(int restauranteId)
        {
            try
            {
                var menus = _menuItemService.GetMenusByRestauranteId(restauranteId);
                return Ok(menus);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("activos/restaurante/{restauranteId}")]
        public IActionResult GetMenusActivosByRestaurante(int restauranteId)
        {
            try
            {
                var result = _menuItemService.GetMenusActivosByRestauranteId(restauranteId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPut("softdelete/{id}")]
        public async Task<IActionResult> SoftDeleteMenuItem(int id)
        {
            try
            {
                var result = await _menuItemService.SoftDeleteMenuItem(id);
                return Ok(new { message = "Menú eliminado correctamente." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPut("restaurar/{id}")]
        public async Task<IActionResult> RestaurarMenuItem(int id)
        {
            try
            {
                var result = await _menuItemService.RestaurarMenuItem(id);
                return Ok(new { message = "Menú restaurado correctamente." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }





    }
}
