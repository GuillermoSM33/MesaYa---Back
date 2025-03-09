using MesaYa.Data;
using MesaYa.Interfaces;
using MesaYa.Models;

namespace MesaYa.Services
{
    public class UsuarioServices : IUsuarioServices
    {
        private readonly ApplicationDbContext _context;

        public UsuarioServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public Usuario GetUserById(int id)
        {
            return _context.Usuarios.Find(id);
        }

        public void CreateUser(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }
    }
}
