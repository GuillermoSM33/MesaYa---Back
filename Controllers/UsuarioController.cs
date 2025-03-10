using MesaYa.Interfaces;
using MesaYa.Models;
using Microsoft.AspNetCore.Mvc;

namespace MesaYa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioServices _usuarioServices;

        public UsuarioController(IUsuarioServices usuarioServices)
        {
            _usuarioServices = usuarioServices;
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
    }
}
