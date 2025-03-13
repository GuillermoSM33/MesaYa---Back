using MesaYa.Models;

namespace MesaYa.Interfaces
{
    public interface IAuthService
    {
        string GenerateJWTToken(string username, string email, string role);
        Usuario ValidateUser(string username, string password);
    }
}
