using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MesaYa.Data;
using MesaYa.Interfaces;
using MesaYa.Models;
using Microsoft.EntityFrameworkCore;

namespace MesaYa.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

            // Imprimir todas las claves de configuración
            foreach (var kvp in _configuration.AsEnumerable())
            {
                Console.WriteLine($"Clave: {kvp.Key}, Valor: {kvp.Value}");
            }
        }
        public Usuario ValidateUser(string email, string password)
        {
            var user = _context.Usuarios
                .Include(u => u.UsuarioAsRoles)  // Declaramos que hay una relación con UsuarioAsRoles
                .ThenInclude(ur => ur.Role)       // Incluimos la información del Role
                .FirstOrDefault(u => u.Email == email && !u.IsDeleted);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public string GenerateJWTToken(string username, string email, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, username),
        new Claim(JwtRegisteredClaimNames.Email, email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Role, role)  // Aquí ya pasamos el rol real para que se genere bien en el token
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiryMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            var secretKey = _configuration["Jwt:Key"];
            Console.WriteLine($"JWT Key: {secretKey}");
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            var computedHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(inputPassword)));
            return storedHash == computedHash;
        }

    }
}
