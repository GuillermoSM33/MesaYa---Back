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




    }
}
