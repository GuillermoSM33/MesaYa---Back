using MesaYa.Models;

namespace MesaYa.Interfaces
{
    public interface IAuthService
    {
        string GenerateJWTToken(string username, string email, string role, int usuarioId); //inclimos el id del usuario para generarlo bien
        Usuario ValidateUser(string username, string password);
    }
}
