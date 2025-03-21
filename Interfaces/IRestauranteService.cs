﻿
using MesaYa.Models;

namespace MesaYa.Interfaces
{
    public interface IRestauranteService
    {

        public List<RestauranteDTO> GetRestaurante();

        public List<RestauranteDTO> GetRestaurantesByTrue();
        public Task<RestauranteDTO> CreateRestaurante(CrearRestauranteDTO crearRestauranteDTO);

        public Task<RestauranteDTO> UpdateRestaurante(int id, UpdateRestauranteDTO updateRestauranteDTO);

        public Task<RestauranteDTO> GetRestauranteById(int id);

        Task<bool> SoftRestoreRestaurante(int id);
        Task<bool> SoftDeleteRestaurante(int id);

    }
}
