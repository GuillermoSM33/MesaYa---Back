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

        [HttpPost("login")]
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

            var token = _authService.GenerateJWTToken(user.Email, user.Username);
            return Ok(new { token });
        }
    }
}
