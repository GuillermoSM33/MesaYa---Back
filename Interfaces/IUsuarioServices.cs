using MesaYa.Models;

namespace MesaYa.Interfaces
{
    public interface IUsuarioServices
    {
        Usuario GetUserById(int id);
        void CreateUser(Usuario usuario);
        Usuario RegisterUser(Usuario usuario);
        string HashPassword(string password);
        bool EmailExists(string email);
    }
}
