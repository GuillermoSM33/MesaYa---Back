using System;
using MesaYa.Data;
using MesaYa.Interfaces;
using MesaYa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MesaYa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioServices _usuarioServices;
        private readonly ApplicationDbContext _context;

        public UsuarioController(IUsuarioServices usuarioServices, ApplicationDbContext context)
        {
            _usuarioServices = usuarioServices;
            _context = context;

        }
        public class RegisterRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Correo y contraseña son obligatorios" });
            }

            if (_usuarioServices.EmailExists(request.Email))
            {
                return Conflict(new { message = "Este correo ya está registrado." });
            }

            var usuario = new Usuario
            {
                Email = request.Email,
                Username = request.Email.Split('@')[0],
                PasswordHash = _usuarioServices.HashPassword(request.Password),
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            usuario.UsuarioAsRoles = new List<UsuarioAsRole>
            {
                new UsuarioAsRole
                {
                    RoleId = 2 // Usuario por defecto
                }
            };

            var newUser = _usuarioServices.RegisterUser(usuario);
            return Ok(new { message = "Usuario registrado con éxito", newUser });
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _usuarioServices.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }
            return Ok(user);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var allUsers = await _context.Usuarios.ToListAsync();
                return Ok(allUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //Endpoint para obtener a los usuarios con rol de hostess

        [HttpGet("hostess")]
        public async Task<IActionResult> GetHostessUsers()
        {
            try
            {
                var hostessUsers = await _context.Usuarios
                    .Where(u => _context.UsuarioAsRoles.Any(r => r.UsuarioId == u.UsuarioId && r.RoleId == 3))
                    .ToListAsync();

                return Ok(hostessUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
