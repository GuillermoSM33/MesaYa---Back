using System.Threading.Tasks;

namespace MesaYa.Interfaces
{
    public interface IRecaptchaValidator
    {
        Task<bool> ValidateAsync(string recaptchaToken);
    }
}
