using MesaYa.Interfaces;
using MesaYa.Models;
using Microsoft.AspNetCore.Mvc;

namespace MesaYa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        public class LoginRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        /*[HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Correo y contraseña son obligatorios" });
            }

            var user = _authService.ValidateUser(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            var role = user.UsuarioAsRoles.FirstOrDefault()?.Role?.RoleName ?? "User";

            var token = _authService.GenerateJWTToken(user.Email, user.Username, role);
            return Ok(new { token });
        }*/

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Solicitud inválida. Revisa el formato del JSON enviado." });
            }

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "Correo y contraseña son obligatorios." });
            }

            var user = _authService.ValidateUser(request.Email, request.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Credenciales inválidas." });
            }

            if (user.UsuarioAsRoles == null || !user.UsuarioAsRoles.Any())
            {
                return Unauthorized(new { message = "El usuario no tiene roles asignados." });
            }

            var role = user.UsuarioAsRoles.FirstOrDefault()?.Role?.RoleName ?? "User";

            var token = _authService.GenerateJWTToken(user.Email, user.Username, role);

            return Ok(new { token });
        }

    }
}
