using MesaYa.Data;
using MesaYa.Interfaces;
using MesaYa.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace MesaYa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        private readonly ApplicationDbContext _context;

        public AuthController(IAuthService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
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

        [HttpPost("logout")]
        public IActionResult Logout()
        {
          
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest(new { message = "Token no proporcionado o formato incorrecto." });
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
            {
                return BadRequest(new { message = "Token inválido." });
            }

            var jwtToken = handler.ReadJwtToken(token);
            var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            if (string.IsNullOrEmpty(jti))
            {
                return BadRequest(new { message = "No se pudo extraer el Jti del token." });
            }

            var expires = jwtToken.ValidTo; // Fecha y hora UTC de expiración del token

            var revokedToken = new RevokedToken
            {
                Jti = jti,
                RevokedAt = DateTime.UtcNow,
                ExpirationDate = expires
            };

            _context.RevokedTokens.Add(revokedToken);
            _context.SaveChanges();

            return Ok(new { message = "Sesión cerrada. Token revocado." });
        }


    }
}
