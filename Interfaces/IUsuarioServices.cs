using MesaYa.Models;

namespace MesaYa.Interfaces
{
    public interface IUsuarioServices
    {
        Usuario GetUserById(int id);
        void CreateUser(Usuario usuario);
    }
}
