using System;
using MesaYa.Data;
using MesaYa.DTOs;
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

            return Ok(new
            {
                user.UsuarioId,
                user.Username,
                user.Email,
                user.PasswordHash,
                user.IsDeleted,
                Roles = user.UsuarioAsRoles.Select(r => r.RoleId)
            });
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

        //Endpoint para poder editar a un usuario
        [HttpPost("edit-user")]
        public async Task<IActionResult> ActionEditUser([FromBody] UserDataForEditDTO userDto)
        {
            try
            {
                var userToEdit = await _context.Usuarios
                    .Include(u => u.UsuarioAsRoles)
                        .ThenInclude(uar => uar.Role)
                    .FirstOrDefaultAsync(u => u.UsuarioId == userDto.UsuarioId);

                if (userToEdit == null)
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }

                // Actualizar datos del usuario
                userToEdit.Username = userDto.Username;
                userToEdit.Email = userDto.Email;
                userToEdit.PasswordHash = userDto.PasswordHash;
                userToEdit.IsDeleted = userDto.IsDeleted;

                // Actualizar roles
                userToEdit.UsuarioAsRoles.Clear();

                foreach (var roleId in userDto.RoleIds)
                {
                    var roleExists = await _context.Roles.AnyAsync(r => r.RoleId == roleId);
                    if (!roleExists)
                        return BadRequest(new { message = $"El rol con ID {roleId} no existe" });

                    userToEdit.UsuarioAsRoles.Add(new UsuarioAsRole
                    {
                        UsuarioId = userDto.UsuarioId,
                        RoleId = roleId
                    });
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Usuario editado con éxito",
                    user = new
                    {
                        userToEdit.UsuarioId,
                        userToEdit.Username,
                        userToEdit.Email,
                        Roles = userToEdit.UsuarioAsRoles.Select(r => r.RoleId)
                    }
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        //Endpoint para editar su propio perfil por usuario
        [HttpPut("edit-profile/{usuarioId}")]
        public async Task<IActionResult> EditProfile([FromRoute] int usuarioId,
                                                    [FromBody] UserSelfProfileEditDTO userDto)
        {
            try
            {
                // 1. Obtener el usuario
                var userToEdit = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

                if (userToEdit == null)
                    return NotFound(new { message = "Usuario no encontrado" });

                // 2. Verificar duplicado de email si cambió
                if (!string.Equals(userToEdit.Email, userDto.Email, StringComparison.OrdinalIgnoreCase))
                {
                    bool existsEmail = await _context.Usuarios
                        .AnyAsync(u => u.Email == userDto.Email && u.UsuarioId != usuarioId);

                    if (existsEmail)
                        return Conflict(new { message = "El email ya está en uso por otro usuario." });
                }

                // 3. Actualizar campos permitidos
                userToEdit.Username = userDto.Username;
                userToEdit.Email = userDto.Email;

                // 4. Hashear la contraseña si el usuario la envió
                if (!string.IsNullOrWhiteSpace(userDto.Password))
                {
                    userToEdit.PasswordHash = _usuarioServices.HashPassword(userDto.Password);
                }

                // 5. Guardar
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Perfil editado con éxito",
                    user = new
                    {
                        userToEdit.UsuarioId,
                        userToEdit.Username,
                        userToEdit.Email
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}
