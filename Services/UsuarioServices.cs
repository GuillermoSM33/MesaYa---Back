using MesaYa.Data;
using MesaYa.Interfaces;
using MesaYa.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MesaYa.Services
{
    public class UsuarioServices : IUsuarioServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UsuarioServices(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public Usuario GetUserById(int id)
        {
            return _context.Usuarios
                .Include(u => u.UsuarioAsRoles)
                .ThenInclude(uar => uar.Role)
                .FirstOrDefault(u => u.UsuarioId == id);
        }

        public void CreateUser(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }
        public Usuario RegisterUser(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges(); 

            return _context.Usuarios.FirstOrDefault(u => u.Email == usuario.Email);
        }
        public string HashPassword(string password)
        {
            var secretKey = _configuration["Jwt:Key"]; 
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
            return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
        public bool EmailExists(string email)
        {
            return _context.Usuarios.Any(u => u.Email == email);
        }
    }
}
