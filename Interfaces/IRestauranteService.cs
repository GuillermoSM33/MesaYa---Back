
using MesaYa.Models;

namespace MesaYa.Interfaces
{
    public interface IRestauranteService
    {

        public List<Restaurante> GetRestaurante();

        public Task<RestauranteDTO> CreateRestaurante(CrearRestauranteDTO crearRestauranteDTO);

        public Task<Restaurante> GetRestauranteById(int id);
        Task<bool> SoftDeleteRestaurante(int id);

    }
}
