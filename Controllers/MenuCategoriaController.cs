using MesaYa.DTOs;
using MesaYa.Interfaces;
using MesaYa.Models;
using Microsoft.AspNetCore.Mvc;

namespace MesaYa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuCategoriaController :Controller
    {
        private readonly IMenuCategoriaService _menuCategoriaService;
        public MenuCategoriaController(IMenuCategoriaService menuCategoriaService) 
        {
            _menuCategoriaService = menuCategoriaService;
        }


        [HttpGet]
public IActionResult GetCategorias()
{
    try
    {
        var categorias = _menuCategoriaService.GetCategorias(); // Asegúrate de que esté con paréntesis
        return Ok(categorias);
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
}


        [HttpPost]
        public async Task<IActionResult> CreateCategoria([FromBody] MenuCategoria menuCategoria)
        {
            try
            {
                var categoria = await _menuCategoriaService.CreateCategoria(menuCategoria); // ✅ Usa await aquí
                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("crear-multiples")]
        public async Task<IActionResult> CrearCategoriasMultiples([FromBody] CrearCategoriasMultiplesDTO dto)
        {
            try
            {
                var categorias = await _menuCategoriaService.CrearCategoriasMultiples(dto);
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateCategoria(int id, [FromBody] MenuCategoria menuCategoria)
        {
            try
            {
                var result = await _menuCategoriaService.UpdateCategoria(id, menuCategoria);
                    return Ok(result);
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
